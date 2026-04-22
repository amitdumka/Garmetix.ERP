# scripts\setup-and-sign.ps1
Param(
  [string]$PfxPasswordPlain = ""
)

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
if (-not (Test-Path $repoRoot)) { $repoRoot = Get-Location }

Write-Host "Repo root: $repoRoot"

# Ensure certs folder
$certDir = Join-Path $repoRoot "certs"
New-Item -Path $certDir -ItemType Directory -Force | Out-Null

# Determine password
if ([string]::IsNullOrEmpty($PfxPasswordPlain)) {
  # generate random 20-char password
  $plain = [System.Guid]::NewGuid().ToString("N") + [System.Guid]::NewGuid().ToString("N")
  $plain = $plain.Substring(0,20)
  Write-Host "No password provided; generated random password."
} else {
  $plain = $PfxPasswordPlain
}

$securePass = ConvertTo-SecureString -String $plain -AsPlainText -Force

# Create self-signed code-signing certificate (CurrentUser store)
Write-Host "Creating self-signed Code Signing cert..."
$cert = New-SelfSignedCertificate -Type CodeSigning `
  -Subject "CN=Garmetix-Dev-CodeSign" `
  -KeyExportPolicy Exportable `
  -KeySpec Signature `
  -CertStoreLocation "Cert:\CurrentUser\My" `
  -NotAfter (Get-Date).AddYears(5)

if (-not $cert) {
  Write-Error "Certificate creation failed."
  exit 1
}

$pfxPath = Join-Path $certDir "codesign.pfx"
Export-PfxCertificate -Cert $cert -FilePath $pfxPath -Password $securePass -Force
Write-Host "Exported PFX to: $pfxPath"

# Persist password in user environment for MSBuild use (use CI secrets in pipelines)
Write-Host "Setting user environment variable CODESIGN_PFX_PASS (use CI secret in production)..."
setx CODESIGN_PFX_PASS $plain | Out-Null
# also set for current process
$env:CODESIGN_PFX_PASS = $plain

# Create Directory.Build.targets for signing
$targetsPath = Join-Path $repoRoot "Directory.Build.targets"
$targetsContent = @"
<Project>
  <PropertyGroup>
    <CodeSignPfx>$(MSBuildThisFileDirectory)certs\codesign.pfx</CodeSignPfx>
    <CodeSignPassword>$(CODESIGN_PFX_PASS)</CodeSignPassword>
    <SignTool>signtool</SignTool>
  </PropertyGroup>

  <Target Name="SignOutputs" AfterTargets="Build" Condition="'$(OS)' == 'Windows_NT' and Exists('$(CodeSignPfx)')">
    <ItemGroup>
      <SignFiles Include="$(TargetPath)" />
      <SignFiles Include="$(TargetDir)**\*.dll" />
      <SignFiles Include="$(TargetDir)**\*.exe" />
    </ItemGroup>

    <Exec Command="&quot;$(SignTool)&quot; sign /fd SHA256 /f &quot;$(CodeSignPfx)&quot; /p &quot;$(CodeSignPassword)&quot; &quot;%(SignFiles.Identity)&quot;" />
  </Target>
</Project>
"@

Write-Host "Writing Directory.Build.targets to $targetsPath"
Set-Content -Path $targetsPath -Value $targetsContent -Encoding UTF8

# Locate signtool
$signtoolCmd = Get-Command signtool.exe -ErrorAction SilentlyContinue
if (-not $signtoolCmd) {
  Write-Warning "signtool.exe not found in PATH. Install Windows SDK or add signtool to PATH. The Directory.Build.targets will still be created."
} else {
  Write-Host "Found signtool at $($signtoolCmd.Source)"
}

# Build solution (adjust configuration if needed)
Write-Host "Starting dotnet build (Debug)..."
dotnet build -c Debug

# Verify signatures of found outputs
Write-Host "Verifying signatures for built files named 'Garmetix.Data.dll' (searching under repo)..."
$built = Get-ChildItem -Path $repoRoot -Filter "Garmetix.Data.dll" -Recurse -ErrorAction SilentlyContinue
if ($built.Count -eq 0) {
  Write-Warning "No built Garmetix.Data.dll found. Build may have failed or target runtime differs."
} else {
  foreach ($f in $built) {
    Write-Host "Checking: $($f.FullName)"
    $sig = Get-AuthenticodeSignature -FilePath $f.FullName
    Write-Host "  Status: $($sig.Status)  SignerCertificate.Subject: $($sig.SignerCertificate.Subject)"
  }
}

Write-Host "Done. Keep the PFX secure. For production use a CA-issued cert and CI secrets."
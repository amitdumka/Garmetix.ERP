using Garmetix.Authentication.Services;
using Garmetix.Core.Auth; 
using Garmetix.Core.Models.Stores;
using Garmetix.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Garmetix.UI.Seeders
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public DatabaseSeeder(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task SeedAsync()
        {
            // 1. Seed Company
            var company = await _context.Companies.FirstOrDefaultAsync();
            if (company == null)
            {
                company = new Company { Name = "Aadwika Fashion" };
                await _context.Companies.AddAsync(company);
                await _context.SaveChangesAsync();
            }

            // 2. Seed Store Group
            var storeGroup = await _context.StoreGroups.FirstOrDefaultAsync();
            if (storeGroup == null)
            {
                storeGroup = new StoreGroup
                {
                    Name = "MBO",
                    CompanyId = company.Id
                };
                await _context.StoreGroups.AddAsync(storeGroup);
                await _context.SaveChangesAsync();
            }

            // 3. Seed Store
            var store = await _context.Stores.FirstOrDefaultAsync();
            if (store == null)
            {
                store = new Store
                {
                    Name = "Aadwika Fashion",
                    StoreGroupId = storeGroup.Id,
                    CompanyId = company.Id
                };
                await _context.Stores.AddAsync(store);
                await _context.SaveChangesAsync();
            }

            // 4. Seed Default Admin User
            var adminUser = await _context.Users.FirstOrDefaultAsync();
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    Name = "Ajay Kumar", // System Owner
                    UserName = "admin",
                    PasswordHash = _authService.HashSecret("admin123"), // Securely hash default password
                    Role = LoginRole.Admin,
                    UserType = UserType.Owner,
                    AppOperation = AppOperation.All,

                    // Link user to the newly seeded hierarchy
                    CompanyId = company.Id,
                    StoreGroupId = storeGroup.Id,
                    StoreId = store.Id,

                    Admin = true
                };
                await _context.Users.AddAsync(adminUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Garmetix.Core.Models;
using Garmetix.Core.Models.Accounting;
using Garmetix.Core.Interfaces;
using Garmetix.Core;

namespace Garmetix.UI.Services
{
    public class MasterDataService
    {
        private readonly IServiceProvider _serviceProvider;

        public MasterDataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // 1. Fetch Data Dynamically
        public async Task<IEnumerable<object>> GetAllAsync(string entityName)
        {
            return entityName switch
            {
                "Party" => await _serviceProvider.GetService<IRepository<Party>>().GetAllAsync(),
                "Ledger" => await _serviceProvider.GetService<IRepository<Ledger>>().GetAllAsync(),
                "LedgerGroup" => await _serviceProvider.GetService<IRepository<LedgerGroup>>().GetAllAsync(),
                "Bank" => await _serviceProvider.GetService<IRepository<Bank>>().GetAllAsync(),
                "BankAccount" => await _serviceProvider.GetService<IRepository<BankAccount>>().GetAllAsync(),
                _ => throw new ArgumentException($"Unknown entity: {entityName}")
            };
        }

        // 2. Create Empty Object Dynamically
        public object CreateNewEntity(string entityName)
        {
            return entityName switch
            {
                "Party" => new Party { Name = "", Category = PartyType.Customer },
                "Ledger" => new Ledger { Name = "", OpenningDate = DateTime.Now },
                "LedgerGroup" => new LedgerGroup { Name = "" },
                "Bank" => new Bank { Name = "" },
                "BankAccount" => new BankAccount { OpeningDate = DateTime.Now },
                _ => throw new ArgumentException($"Unknown entity: {entityName}")
            };
        }

        // 3. Save Dynamically
        public async Task SaveAsync(string entityName, object entity, bool isNew)
        {
            switch (entityName)
            {
                case "Party": await SaveEntityAsync(_serviceProvider.GetService<IRepository<Party>>(), (Party)entity, isNew); break;
                case "Ledger": await SaveEntityAsync(_serviceProvider.GetService<IRepository<Ledger>>(), (Ledger)entity, isNew); break;
                case "LedgerGroup": await SaveEntityAsync(_serviceProvider.GetService<IRepository<LedgerGroup>>(), (LedgerGroup)entity, isNew); break;
                case "Bank": await SaveEntityAsync(_serviceProvider.GetService<IRepository<Bank>>(), (Bank)entity, isNew); break;
                case "BankAccount": await SaveEntityAsync(_serviceProvider.GetService<IRepository<BankAccount>>(), (BankAccount)entity, isNew); break;
                default: throw new ArgumentException($"Unknown entity: {entityName}");
            }
        }

        // 4. Delete Dynamically
        public async Task DeleteAsync(string entityName, object entity)
        {
            switch (entityName)
            {
                case "Party": await _serviceProvider.GetService<IRepository<Party>>().DeleteAsync((Party)entity); break;
                case "Ledger": await _serviceProvider.GetService<IRepository<Ledger>>().DeleteAsync((Ledger)entity); break;
                // ... map the rest ...
            }
        }

        private async Task SaveEntityAsync<T>(IRepository<T> repo, T entity, bool isNew) where T : BaseEntity
        {
            if (isNew) await repo.AddAsync(entity);
            else await repo.UpdateAsync(entity);
        }
    }
}
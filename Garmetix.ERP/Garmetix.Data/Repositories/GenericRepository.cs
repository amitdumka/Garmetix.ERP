using Garmetix.Core.Interfaces;
using Garmetix.Core.Models;
using Garmetix.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace Garmetix.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // ... inside GenericRepository<T> class ...

        private void ValidateEntity(T entity)
        {
            var validationContext = new ValidationContext(entity, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResults, validateAllProperties: true);

            if (!isValid)
            {
                // Throws a clean error message that BaseViewModel will catch and show to the user!
                throw new ValidationException(validationResults.First().ErrorMessage);
            }
        }

        // Update your Add and Update methods:
        public async Task AddAsync(T entity)
        {
            ValidateEntity(entity); // <--- Add this!

            entity.CreatedAt = DateTime.UtcNow;
            entity.Synced = false;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            ValidateEntity(entity); // <--- Add this!

            entity.UpdatedAt = DateTime.UtcNow;
            entity.Synced = false;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<T> GetByIdAsync(Guid id)
                => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.FirstOrDefaultAsync(predicate);

        //public async Task AddAsync(T entity)
        //{
        //    entity.CreatedAt = DateTime.UtcNow;
        //    entity.Synced = false; // Needs to be pushed to cloud
        //    await _dbSet.AddAsync(entity);
        //    await _context.SaveChangesAsync();
        //}

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            var baseEntities = entities.ToList();
            foreach (var entity in baseEntities)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.Synced = false;
            }
            await _dbSet.AddRangeAsync(baseEntities);
            await _context.SaveChangesAsync();
        }

        //public async Task UpdateAsync(T entity)
        //{
        //    entity.UpdatedAt = DateTime.UtcNow;
        //    entity.Synced = false; // Flag as unsynced because it was modified
        //    _dbSet.Update(entity);
        //    await _context.SaveChangesAsync();
        //}

        public async Task DeleteAsync(T entity)
        {
            // Soft Delete Implementation
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.Synced = false; // The cloud needs to know it was deleted

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task HardDeleteAsync(T entity)
        {
            // Truly wipes it from the SQLite file
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task ExecuteTransactionAsync(Func<Task> action)
        {
            // Used for complex POS operations (e.g., saving Invoice + updating Stock simultaneously)
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
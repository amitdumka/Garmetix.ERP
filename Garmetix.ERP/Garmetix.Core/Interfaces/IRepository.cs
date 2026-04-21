using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Garmetix.Core.Models;

namespace Garmetix.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        
        Task UpdateAsync(T entity);
        
        Task DeleteAsync(T entity); // Soft delete
        Task HardDeleteAsync(T entity);
        
        Task ExecuteTransactionAsync(Func<Task> action); // For atomic operations like Sale + Stock deduction
    }
}
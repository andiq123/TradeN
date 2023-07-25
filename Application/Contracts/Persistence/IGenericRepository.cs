using System.Linq.Expressions;
using Core.Entities;

namespace Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(Guid id, Expression<Func<T, object>>[]? includes = null);
    Task<T?> GetFirstOrDefault(Expression<Func<T, bool>>? where = null);
    Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, bool>>? where = null,
        Expression<Func<T, object>>[]? includes = null, Expression<Func<T, object>>[]? ordersBy = null,
        bool descending = false);
    Task<Guid> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
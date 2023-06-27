using System.Linq.Expressions;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly TradeNContext _context;

    public GenericRepository(TradeNContext context)
    {
        _context = context;
    }

    public async Task<T> GetByIdAsync(Guid id, Expression<Func<T, object>>[]? includes = null)
    {
        var query = _context.Set<T>().AsQueryable();

        if (includes is not null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new NotFoundException(typeof(T).Name, id);
        }

        return entity;
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, bool>>? where = null,
        Expression<Func<T, object>>[]? includes = null,
        Expression<Func<T, object>>[]? ordersBy = null,
        bool descending = false)
    {
        var query = _context.Set<T>().AsQueryable();

        if (where is not null)
        {
            query = query.Where(where);
        }

        if (includes is not null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (ordersBy is not null && ordersBy.Length > 0)
        {
            var orderedQuery = descending
                ? query.OrderByDescending(ordersBy[0])
                : query.OrderBy(ordersBy[0]);

            for (var i = 1; i < ordersBy.Length; i++)
            {
                orderedQuery = descending
                    ? orderedQuery.ThenByDescending(ordersBy[i])
                    : orderedQuery.ThenBy(ordersBy[i]);
            }

            query = orderedQuery;
        }

        var entities = await query.ToListAsync();

        if (entities is null || entities.Count == 0)
            throw new NotFoundException(typeof(T).Name);

        return entities;
    }


    public async Task<Guid> AddAsync(T entity)
    {
        var result = await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
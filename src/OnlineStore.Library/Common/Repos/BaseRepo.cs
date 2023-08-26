using Microsoft.EntityFrameworkCore;
using OnlineStore.Library.Common.Interfaces;
using OnlineStore.Library.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Library.Common.Repos;

public abstract class BaseRepo<T> : IRepo<T> where T : class, IIdentifiable, new()
{
    protected DbSet<T> Table;

    protected BaseRepo(OrdersDbContext context)
    {
        Context = context;
        Table = Context.Set<T>();
    }

    protected OrdersDbContext Context { get; init; }

    public async Task<Guid> AddAsync(T entity)
    {
        await Table.AddAsync(entity);
        await SaveChangesAsync();
        return entity.Id;
    }

    public async Task<IEnumerable<Guid>> AddRangeAsync(IEnumerable<T> entities)
    {
        await Table.AddRangeAsync(entities);
        await SaveChangesAsync();

        var result = new List<Guid>(entities.Select(e => e.Id));
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await GetOneAsync(id);

        if (entity != null)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return await SaveChangesAsync();
        }

        return 0;
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<Guid> ids)
    {
        var result = 0;

        foreach (var id in ids)
        {
            var affectedValue = await DeleteAsync(id);
            result += affectedValue;
        }

        return result;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Table.ToListAsync();
    }

    public virtual async Task<T> GetOneAsync(Guid id)
    {
        return await Table.FindAsync(id);
    }

    public async Task<int> SaveAsync(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        return await SaveChangesAsync();
    }

    internal async Task<int> SaveChangesAsync()
    {
        try
        {
            return await Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        catch (DbUpdateException)
        {
            throw;
        }
    }
}
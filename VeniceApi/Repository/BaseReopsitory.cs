﻿using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using VeniceApi.Interfaces;

namespace VeniceApi.Repository;

public class BaseReopsitory<T> : IRepository<T>  where T : class
{
    private readonly OTContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseReopsitory(OTContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public  async Task<T> Add(T entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();

    }

    public virtual async Task<T> GetById(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}

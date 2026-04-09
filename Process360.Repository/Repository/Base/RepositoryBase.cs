using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Repository.Interface;

namespace Process360.Repository.Repository.Base;

/// <summary>
/// Base repository class providing CRUD operations for all entities
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly ProcessDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(ProcessDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    #region Create

    public virtual async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        return await CreateAsync(entity);
    }

    public virtual void Add(T entity)
    {
        Create(entity);
    }

    #endregion

    #region Read

    public virtual async Task<T?> GetDetailsByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual T? GetDetailsById(int id)
    {
        return _dbSet.Find(id);
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await GetDetailsByIdAsync(id);
    }

    public virtual T? GetById(int id)
    {
        return GetDetailsById(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    #endregion

    #region Update/Edit

    public virtual async Task<T> EditAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual void Edit(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        return await EditAsync(entity);
    }

    public virtual void Update(T entity)
    {
        Edit(entity);
    }

    #endregion

    #region Delete

    public virtual async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetDetailsByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public virtual bool Delete(int id)
    {
        var entity = GetDetailsById(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public virtual void DeleteEntity(T entity)
    {
        _dbSet.Remove(entity);
    }

    #endregion

    #region Save Changes

    public virtual async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await SaveAsync();
    }

    public virtual int Save()
    {
        return _context.SaveChanges();
    }

    public virtual int SaveChanges()
    {
        return Save();
    }

    #endregion
}

namespace Process360.Repository.Interface;

/// <summary>
/// Generic repository interface providing CRUD operations for all entities
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : class
{
    // Create
    Task<T> CreateAsync(T entity);
    void Create(T entity);
    Task<T> AddAsync(T entity);
    void Add(T entity);

    // Read
    Task<T?> GetDetailsByIdAsync(int id);
    T? GetDetailsById(int id);
    Task<T?> GetByIdAsync(int id);
    T? GetById(int id);
    Task<IEnumerable<T>> GetAllAsync();
    IEnumerable<T> GetAll();

    // Update/Edit
    Task<T> EditAsync(T entity);
    void Edit(T entity);
    Task<T> UpdateAsync(T entity);
    void Update(T entity);

    // Delete
    Task<bool> DeleteAsync(int id);
    bool Delete(int id);
    void DeleteEntity(T entity);

    // Save changes
    Task<int> SaveAsync();
    Task<int> SaveChangesAsync();
    int Save();
    int SaveChanges();
}

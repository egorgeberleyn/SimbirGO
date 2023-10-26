using Microsoft.EntityFrameworkCore;
using Simbir.GO.Shared.Entities;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Shared.Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly DbContext DbContext;

    protected Repository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public Task<List<TEntity>> GetAllByAsync(Specification<TEntity> spec)
    {
        return SpecificationQueryBuilder
            .GetQuery(DbContext.Set<TEntity>(), spec)
            .ToListAsync();
    }

    public Task<TEntity?> GetByAsync(Specification<TEntity> spec)
    {
        return SpecificationQueryBuilder
            .GetQuery(DbContext.Set<TEntity>(), spec)
            .FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(long id) =>
        await DbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
    
    public async Task AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
    }
    
    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }
    
    public void Update(TEntity updatedEntity)
    {
        DbContext.Set<TEntity>().Update(updatedEntity);
    }
    
    public void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }
}
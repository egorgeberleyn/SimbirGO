using Simbir.GO.Shared.Entities;
using Simbir.GO.Shared.Persistence.Specifications;

namespace Simbir.GO.Shared.Persistence.Repositories;

public interface IRepository<TEntity>
    where TEntity: Entity
{
    Task<List<TEntity>> GetAllByAsync(Specification<TEntity> spec);
    Task<TEntity?> GetByAsync(Specification<TEntity> spec);
    Task<TEntity?> GetByIdAsync(long id);
    
    Task AddAsync(TEntity entity);
    void Add(TEntity entity);
    void Update(TEntity updatedEntity);
    void Delete(TEntity entity);
}
using Microsoft.EntityFrameworkCore;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Shared.Persistence.Specifications;

public static class SpecificationQueryBuilder
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQuery,
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        var query = inputQuery;

        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        if (specification.Includes != null)
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        if (specification.OrderBy != null)
            query = query.OrderBy(specification.OrderBy);

        if (specification.OrderByDesc != null)
            query = query.OrderByDescending(specification.OrderByDesc);
        
        if (specification.Take != null)
            query = query.Take(specification.Take.Value);
        
        if (specification.Skip != null)
            query = query.Skip(specification.Skip.Value);

        return query;
    }
}
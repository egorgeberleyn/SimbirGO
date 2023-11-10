using System.Linq.Expressions;
using Simbir.GO.Shared.Entities;

namespace Simbir.GO.Shared.Persistence.Specifications;

public abstract class Specification<TEntity>
    where TEntity : Entity
{
    public Expression<Func<TEntity, bool>>? Criteria { get; }

    public List<Expression<Func<TEntity, object>>>? Includes { get; } = new();
    
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    
    public Expression<Func<TEntity, object>>? OrderByDesc { get; private set; }
    
    public int? Skip { get; private set; }
    
    public int? Take { get; private set; }

    protected Specification()
    {
    }

    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes?.Add(includeExpression);
    }
    
    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    
    protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpression)
    {
        OrderByDesc = orderByDescExpression;
    }
    
    protected void AddSkip(int skipExpression)
    {
        Skip = skipExpression;
    }
    
    protected void AddTake(int takeExpression)
    {
        Take = takeExpression;
    }
}
using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;

namespace SOL.Gateway.Schema.Common;

public class QueryResolver<TView> where TView : class
{
    public IQueryable<TView> Results([Service] IDomainQuery<TView> query) 
        => query.Query;
    
    public IQueryable<TView> OrderedResults([Service] IDomainQuery<TView> query) 
        => query.Query.OrderBy(x => EF.Property<TView>(x, "Order"));
}
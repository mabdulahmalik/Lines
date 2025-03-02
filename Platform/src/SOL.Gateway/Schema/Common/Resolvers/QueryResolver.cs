using Microsoft.EntityFrameworkCore;
using SOL.Abstractions.Persistence;

namespace SOL.Gateway.Schema.Common;

public class QueryResolver<TView> where TView : class
{
    public IQueryable<TView> Results(LinesDataStore dataStore) 
        => dataStore.Set<TView>();
    
    public IQueryable<TView> OrderedResults(LinesDataStore dataStore) 
        => dataStore.Set<TView>().OrderBy(x => EF.Property<TView>(x, "Order"));
}
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SOL.Abstractions.Domain;

namespace SOL.DataAccess.Conventions;

public class DomainModelConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var conventionProperty in entityType.GetProperties()) 
            {
                conventionProperty.SetValueGenerated(ValueGenerated.Never);
            }

            if (entityType.ClrType.IsAssignableTo(typeof(Entity)))
            {
                var existingKeys = entityType.GetDeclaredKeys();
                if (!existingKeys.Any(x => x.IsPrimaryKey()))
                {
                    var idProperty = entityType.FindProperty(nameof(Entity.Id));
                    if (idProperty != null) {
                        entityType.SetPrimaryKey(idProperty);
                    }
                }
            }

            if (entityType.ClrType.IsAssignableTo(typeof(AggregateRoot)))
            {
                entityType.AddIgnored(nameof(AggregateRoot.Changes));       
            }
        }
    }
}
using NodaTime;
using SOL.Gateway.Schema.Common;
using SOL.Utility.Geography;

namespace SOL.Gateway.Schema;

public class Query : ObjectType
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("timeZones")
            .Description("Gets all Time Zones.")
            .Type<ListType<StringType>>()
            .Resolve(ctx => {
                var usaZones = DateTimeZoneProviders.Tzdb.Ids
                    .Where(tz => tz.StartsWith("US/")).ToList();
                var canZones = DateTimeZoneProviders.Tzdb.Ids
                    .Where(tz => tz.StartsWith("Canada/")).ToList();
                
                return usaZones.Union(canZones);
            });

        descriptor
            .Field("states")
            .Description("Gets all US States.")
            .Type<ListType<USStateType>>()
            .Resolve(ctx => USState.GetAll());
    }
}
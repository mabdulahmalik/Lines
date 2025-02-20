using SOL.Service.OrganizationMgmt.Routine.Views;

namespace SOL.Gateway.Schema.OrganizationMgmt;

public class RollingScheduleType : ObjectType<RollingScheduleView>
{
    protected override void Configure(IObjectTypeDescriptor<RollingScheduleView> descriptor)
    {
        descriptor
            .Name("RollingSchedule")
            .Description("A Rolling Interval Schedule.");

        descriptor
            .Field("interval")
            .Description("The interval value for the Routine's Rolling Schedule.")
            .Type<RollingScheduleDurationType>()
            .Resolve(ctx => {
                var routineRolling = ctx.Parent<RollingScheduleView>();
                return new RollingScheduleDurationView { 
                    Value = routineRolling.IntervalValue, 
                    Unit = routineRolling.IntervalUnit 
                };
            });
            
        descriptor
            .Field("delay")
            .Description("The delay value for the Routine's Rolling Schedule.")
            .Type<RollingScheduleDurationType>()
            .Resolve(ctx => {
                var routineRolling = ctx.Parent<RollingScheduleView>();
                return new RollingScheduleDurationView { 
                    Value = routineRolling.DelayValue, 
                    Unit = routineRolling.DelayUnit 
                };
            });           
    }
}
namespace SOL.Abstractions.Activity;

[AttributeUsage(AttributeTargets.Class)]
public class TrackableActivity : Attribute
{
    public TrackableActivity(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
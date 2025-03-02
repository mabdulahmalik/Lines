namespace SOL.Abstractions.Application;

public class OperationContextFactory : IOperationContextFactory
{
    private static readonly AsyncLocal<IOperationContext> Current = new();

    public bool HasValue => Current.Value != null;
    
    public IOperationContext Get()
    {
        return Current.Value ?? throw new InvalidOperationException("OperationContext is not set.");
    }

    public void Set(IOperationContext context)
    {
        Current.Value = context;
    }
}
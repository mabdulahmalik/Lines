namespace SOL.Abstractions.Application;

public interface IOperationContextFactory
{
    bool HasValue { get; }
    IOperationContext Get();
    void Set(IOperationContext context);
}
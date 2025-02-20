using Dawn;

namespace SOL.Abstractions.Domain;

public static class GuardExtensions
{
    public static Guard.ArgumentInfo<T> Invariant<T>(this Guard.ArgumentInfo<T> argInfo, Func<T, bool> predicate, Func<T, string>? message = null)
    {
        return argInfo.Require<InvariantViolationException>(predicate, message);
    }
}
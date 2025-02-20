using MassTransit;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace SOL.Messaging.Infrastructure;

public abstract class CommandHandler<TCommand> : IConsumer<TCommand>, IServiceHandler
    where TCommand : class
{
    protected readonly ILogger Logger;

    protected CommandHandler(ILogger logger)
    {
        Logger = logger;
    }

    public async Task Consume(ConsumeContext<TCommand> context)
    {
        //var trxnCtx = context.GetPayload<TransactionContext>();

        try
        {
            await HandleAsync(context.Message, context.CancellationToken);
            //await trxnCtx.Commit();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error executing command: {Command}", context.Message);
            //trxnCtx.Rollback(ex);
            throw;
        }
    }

    protected abstract Task HandleAsync(TCommand command, CancellationToken stoppageToken);
}
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace SOL.Messaging.Infrastructure.Filters;

class CommandValidationFilter<T> : IFilter<PublishContext<T>>
    where T : class
{
    private readonly IEnumerable<IValidator> _validators;
    private readonly ILogger<CommandValidationFilter<T>> _logger;

    public CommandValidationFilter(IEnumerable<IValidator> validators, ILogger<CommandValidationFilter<T>> logger)
    {
        _validators = validators;
        _logger = logger;
    }
    
    public void Probe(ProbeContext context) { }

    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        var messageType = context.Message.GetType();
        
        using var logScope = _logger.BeginScope(context.Message);
        _logger.LogInformation("[CommandValidation] Message: {MessageType} --- CorrelationId: {CorrelationId} --- ConversationId: {ConversationId} -- ThreadId: {ThreadId}"
            , messageType.Name, context.CorrelationId, context.ConversationId, Thread.CurrentThread.ManagedThreadId);        
        
        var validationFailures = new List<ValidationFailure>();
        var commandValidators = _validators
            .Where(x => x.CanValidateInstancesOfType(messageType))
            .ToList();

        foreach (var commandValidator in commandValidators)
        {
            var validationContext = new ValidationContext<T>(context.Message);
            var validationResults = await commandValidator.ValidateAsync(validationContext);

            if (!validationResults.IsValid)
            {
                validationFailures.AddRange(validationResults.Errors);
            }
        }

        if (validationFailures.Any())
        {
            var ex = new ValidationException(validationFailures);
            
            _logger.LogWarning(ex,"Command Validation Failure for: {CommandName}", messageType.Name);
            throw ex;
        }        
        
        await next.Send(context);
    }
}
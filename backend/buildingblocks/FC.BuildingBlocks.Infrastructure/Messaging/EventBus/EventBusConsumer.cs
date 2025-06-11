using System.Diagnostics;
using System.Text.Json;
using FC.BuildingBlocks.Domain.Messaging;
using FC.BuildingBlocks.Domain.Messaging.EventBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Context;

namespace FC.BuildingBlocks.Infrastructure.Messaging.EventBus
{
    public class EventBusConsumer : IEventBusConsumer
    {
        private readonly ILogger<EventBusConsumer> _logger;
        private readonly IEventBusDispatcher _dispatcher;
        private readonly IOptions<EventBusConsumerSettings> _options;
        private readonly ICorrelationContext _correlationContext;
        private readonly IConsumerWrapper _consumer;

        public EventBusConsumer(
            IEventBusDispatcher dispatcher,
            IOptions<EventBusConsumerSettings> options,
            ILogger<EventBusConsumer> logger,
            ICorrelationContext correlationContext,
            IConsumerWrapper consumer)
        {
            _dispatcher = dispatcher;
            _options = options;
            _logger = logger;
            _correlationContext = correlationContext;
            _consumer = consumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var settings = _options.Value;

            _consumer.Subscribe();

            _logger.LogInformation("Iniciando consumo do tópico EventBus: {Topic}", settings.Topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var rawMessage = _consumer.Consume(cancellationToken);

                    if (string.IsNullOrWhiteSpace(rawMessage))
                    {
                        _logger.LogWarning("Mensagem consumida é nula.");
                        continue;
                    }

                    string? correlationId = null;
                    try
                    {
                        var doc = JsonDocument.Parse(rawMessage);
                        if (doc.RootElement.TryGetProperty("CorrelationId", out var cidProp))
                            correlationId = cidProp.GetString();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogDebug(ex, "Falha ao extrair CorrelationId da mensagem EventBus.");
                    }

                    correlationId ??= Guid.NewGuid().ToString();
                    _correlationContext.Set(correlationId);

                    using (LogContext.PushProperty("CorrelationId", correlationId))
                    using (var activity = new Activity("EventBusMessage"))
                    {
                        activity.SetIdFormat(ActivityIdFormat.W3C);
                        activity.AddTag("CorrelationId", correlationId);
                        activity.Start();

                        _logger.LogInformation("Mensagem consumida do tópico {Topic}", settings.Topic);

                        await _dispatcher.DispatchAsync(rawMessage, cancellationToken);

                        activity.Stop();
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Cancelamento solicitado, encerrando consumidor EventBus.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado no consumidor.");
                }
            }

            _consumer.Close();
        }
    }
}

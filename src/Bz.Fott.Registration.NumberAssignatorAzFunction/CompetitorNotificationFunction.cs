using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

public class CompetitorNotificationFunction
{
    private readonly ILogger<CompetitorNotificationFunction> _logger;

    public CompetitorNotificationFunction(
        ILogger<CompetitorNotificationFunction> logger)
    {
        _logger = logger;
    }

    [FunctionName("CompetitorNotificationFunction")]
    public async Task Run(
        [ServiceBusTrigger("registration-completed-events-to-registr-service", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage receivedMessage,
        [SignalR(HubName = "notifications", ConnectionStringSetting = "SignalRConnectionString")] IAsyncCollector<SignalRMessage> signalrMessage,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Run CompetitorNotificationFunction");

        var registeredCompetitor = GetMessageContent<CompetitorRegisteredIntegrationEvent>(receivedMessage);

        await SendNotification(registeredCompetitor);

        static T GetMessageContent<T>(ServiceBusReceivedMessage receivedMessage)
        {
            string payload = Encoding.UTF8.GetString(receivedMessage.Body);
            var json = JsonNode.Parse(payload)["message"].ToString();
            return JsonConvert.DeserializeObject<T>(json);
        }

        async Task SendNotification(CompetitorRegisteredIntegrationEvent registeredCompetitor)
        {
            await signalrMessage.AddAsync(new SignalRMessage
            {
                Target = "notifications",
                Arguments = new[]
                {
                    new
                    {
                        NotificationType = "CompetitorRegistered",
                        //registeredCompetitor.RequestId,
                        registeredCompetitor.CompetitorId,
                        registeredCompetitor.CompetitionId,
                        registeredCompetitor.FirstName,
                        registeredCompetitor.LastName,
                        registeredCompetitor.BirthDate,
                        registeredCompetitor.City,
                        registeredCompetitor.PhoneNumber,
                        registeredCompetitor.Number
                    }
                }
            });
        }
    }
}

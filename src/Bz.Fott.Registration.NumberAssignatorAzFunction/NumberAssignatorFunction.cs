using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

public class NumberAssignatorFunction
{
    [FunctionName("NumberAssignator")]
    public void Run([ServiceBusTrigger("registrations", Connection = "ServiceBusConnectionString")]string registrationRequest, ILogger log)
    {
        log.LogInformation($"C# ServiceBus queue trigger function processed message: {registrationRequest}");
    }
}

using MassTransit;
using System.Text.RegularExpressions;

namespace Bz.Fott.Registration.Infrastructure.Messaging;

internal class ShortTypeEntityNameFormatter : IEntityNameFormatter
{
    private const string integrationEventSuffix = "IntegrationEvent";

    public string FormatEntityName<T>()
    {
        var messageType = typeof(T).Name;
        if (messageType.EndsWith(integrationEventSuffix, StringComparison.OrdinalIgnoreCase))
        {
            messageType = messageType.Replace(integrationEventSuffix, string.Empty);
        }
        return Regex.Replace(messageType, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}

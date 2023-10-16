using System.Text.Json.Serialization;

namespace Glufs.Reviews.Domain.Events;

public abstract record Event
{
    [JsonIgnore]
    public abstract string Queue { get; }
}



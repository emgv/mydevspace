using System.Text.Json.Serialization;

namespace Shared.Dtos.AzDevOps.WorkItems;

public class WiqlResponseDto
{
    [JsonPropertyName("workItems")] public IEnumerable<WiqlResponseWorkItem> WorkItems { get; set; }
}

public class WiqlResponseWorkItem
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
}

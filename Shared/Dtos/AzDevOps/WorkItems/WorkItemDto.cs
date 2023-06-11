using System.Text.Json.Serialization;

namespace Shared.Dtos.AzDevOps.WorkItems;

public class WorkItemDto
{
    [JsonPropertyName("id")] public long Id { get; set; }
    [JsonPropertyName("fields")] public WorkItemFieldsDto Fields { get; set; }
}

public class WorkItemFieldsDto
{
    [JsonPropertyName("System.CreatedDate")] public DateTime SystemCreatedDate { get; set; }
    [JsonPropertyName("System.Title")] public string SystemTitle { get; set; }
    [JsonPropertyName("System.TeamProject")] public string SystemTeamProject { get; set; }
    [JsonPropertyName("System.WorkItemType")] public string SystemWorkItemType { get; set; }
    [JsonPropertyName("System.State")] public string SystemState { get; set; }
    [JsonPropertyName("System.AssignedTo")] public WorkItemSystemAssignedTo SystemAssignedTo { get; set; }
}

public class WorkItemSystemAssignedTo
{
    [JsonPropertyName("id")] public string Id { get; set; }
    [JsonPropertyName("displayName")] public string DisplayName { get; set; }
    [JsonPropertyName("uniqueName")] public string UniqueName { get; set; }
}


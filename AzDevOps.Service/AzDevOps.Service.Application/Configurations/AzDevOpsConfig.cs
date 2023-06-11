namespace AzDevOps.Service.Application.Configurations;

public class AzDevOpsConfig
{
    public string PAT { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string Project { get; set; } = string.Empty;
}

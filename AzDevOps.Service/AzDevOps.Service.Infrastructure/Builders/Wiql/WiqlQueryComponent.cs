namespace AzDevOps.Service.Infrastructure.Builders.Wiql;

public class WiqlQueryComponent
{
    public WiqlQueryComponentTypeEnum Type { get; private set; }
    public string WiqlCode { get; private set; }
    public WiqlQueryComponent? LinkedQueryComponent { get; private set; }

    public WiqlQueryComponent(WiqlQueryComponentTypeEnum type, string wiqlCode)
    {
        Type = type;
        WiqlCode = wiqlCode;
    }

    public WiqlQueryComponent(
        WiqlQueryComponentTypeEnum type,
        string wiqlCode,
        WiqlQueryComponent linkedQueryComponent)
    {
        Type = type;
        WiqlCode = wiqlCode;
        LinkedQueryComponent = linkedQueryComponent;
    }
}

namespace Doc.Pulse.Contracts.Communications.V1.ApiInformation.Commands;

public class NotificationsSendEmailCmd
{
    public string? SendToAddress { get; set; }
    public string? Subject { get; set; }
}

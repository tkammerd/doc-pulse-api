using AppDmDoc.SharedKernel.Core.Entities;

namespace Doc.Pulse.Core.Abstractions;

public interface IEmailService
{
    Task SendEmail(string subject, string body, List<NotificationEmailDto> emailList);
}

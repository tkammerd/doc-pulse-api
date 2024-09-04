using Esb.CustomerCommunication;
using Esb.CustomerCommunication.Enums;
using Esb.CustomerCommunication.Exceptions;
using Esb.CustomerCommunication.Interfaces;
using Esb.DelegatR.Abstractions;
using AppDmDoc.SharedKernel.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Doc.Pulse.Core.Abstractions;

namespace Doc.Pulse.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ICcClient ccClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> logger;

    public EmailService(ILogger<EmailService> logger, ICcClient ccClient, IConfiguration configuration)
    {
        this.ccClient = ccClient ?? throw new ArgumentNullException(nameof(ccClient));
        this._configuration = configuration;
        this.logger = logger;
    }

    public async Task SendEmail(string subject, string body, List<NotificationEmailDto> emailList)
    {
        if (emailList == null || emailList.Count == 0) throw new ArgumentNullException(nameof(emailList));
        var replyTo = _configuration["Email:ReplyTo"];

        try
        {
            try
            {
                if (emailList != null && emailList.Count > 0)
                {
                    var set = 100;
                    var setNo = 0;
                    var sendToList = new List<string>();
                    while (emailList.Count > set * setNo)
                    {
                        sendToList.Add(string.Join(";",
                            emailList
                                .Skip(set * setNo)
                                .Take(set)
                                .Select(s => s.Email)
                                .ToList()));
                        setNo++;
                    }
                    foreach (var sendTo in sendToList)
                    {
                        PlainTextEmail plainTextEmail = new()
                        {
                            Destinations = new Destination()
                            {
                                To = sendTo, 
                                Cc = null, //sendTo, // TODO -- throwing null exception from automapper - EmailMappingProfile - StringHelper.Split
                                Bcc = null, // sendTo,
                                ReplyTo = replyTo
                            },
                            Subject = subject,
                            BodyText = body,
                            IsSecure = false,
                            Importance = Importance.Normal,
                            Priority = Priority.Normal,
                            Sensitivity = Sensitivity.Normal,
                            //Expires = null,
                            //ReplyBy = null,
                            //Attachments = null
                        };

                        _ = await ccClient.Emails.Send(plainTextEmail);
                    }
                }
                else
                {
                    logger.LogInformation($"No email addresses in database to send emails to");
                }
            }
            catch (EsbCustomerCommunicationException ex)
            {
                logger.LogError(ex, $"An error occurred while attempting to send email(s). The CC Component threw the error.. Error: {ex.Message}");
            }
            catch (EsbDelegatRException ex)
            {
                logger.LogError(ex, $"An error occurred while attempting to send email(s). The DelegatR Email API Component threw the error.. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An unexpected error occurred while attempting to send email(s). Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred retrieving the list of email addresses.. Error: {ex.Message}");
        }
    }
}

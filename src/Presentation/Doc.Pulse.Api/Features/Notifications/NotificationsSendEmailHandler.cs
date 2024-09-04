using AppDmDoc.SharedKernel.Core.Entities;
using MediatR;
using Doc.Pulse.Core.Abstractions;

namespace Doc.PublicForms.Api.Features.SAR;

public class NotificationsSendEmailHandler 
{
    public class Request : INotification 
    {
        public Guid Reference { get; set; }
        public string Subject { get; set; } = default!;
        public string Body { get; set; } = default!;
        public List<NotificationEmailDto> EmailToList { get; set; } = [];
    }

    public class Handler : INotificationHandler<Request>
    {
        private readonly ILogger<Request> _logger;
        private readonly IEmailService _emailService;
        
        public Handler(ILogger<Request> logger, IEmailService emailService)
        {
            this._logger = logger;
            _emailService = emailService;
        }

        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                await _emailService.SendEmail(request.Subject, request.Body, request.EmailToList);

                _logger.LogDebug("Email Notification Sent for Document #{Reference}.", request.Reference);
            }
            catch (Exception exception)
            {
                _logger.LogError("Failed to send Email Notification due to: {Message}", exception.Message);
            }

            return;
        }
    }
}

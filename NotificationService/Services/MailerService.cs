using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NotificationService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Services
{
    public class MailerService : Mail.MailBase
    {
        private readonly ILogger<MailerService> _logger;
        private IEmailService _emailService;
        public MailerService(ILogger<MailerService> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public override async Task<Response> Send(SendBody request, ServerCallContext context)
        {
            RepeatedField<string> recipients = new RepeatedField<string>
            {
                "divyesh.bhartiya@cesltd.com",
                "divyesh.bhartiya@gmail.com"
            };

            return await _emailService.SendAsync(request.Sender, recipients, request.Subject, request.BodyText);
        }

        //public override async Task<Response> SendAsync(SendBody request, ServerCallContext context)
        //{
        //    RepeatedField<string> recipients = new RepeatedField<string>
        //    {
        //        "divyesh.bhartiya@cesltd.com",
        //        "divyesh.bhartiya@gmail.com"
        //    };

        //    return await _emailService.SendAsync(request.Sender, recipients, request.Subject, request.BodyText);
        //}

    }
}

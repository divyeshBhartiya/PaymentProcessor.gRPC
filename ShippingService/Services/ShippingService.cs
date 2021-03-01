using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NotificationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShippingService
{
    public class ShippingService : ProductShipment.ProductShipmentBase
    {
        private readonly ILogger<ShippingService> _logger;
        private readonly Mail.MailClient _mailClient;
        public ShippingService(ILogger<ShippingService> logger, Mail.MailClient mailClient)
        {
            _logger = logger;
            _mailClient = mailClient;
        }

        public override async Task<SendOrderReply> SendOrder(SendOrderRequest request, ServerCallContext context)
        {
            try
            {
                this._logger.LogInformation($"Sent order {request.OrderId}");

                StringBuilder contentSb = new StringBuilder();
                contentSb.Append("This mail is sent from a POC which involves gRPC communication over Http/2 protocol. ");
                contentSb.Append("The idea is create a scenario of processing payment for a particular order and " +
                    "trigger shippment process for the same. ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append("The POC also implements the mailing service to send notification to the user. ");
                contentSb.Append("This working POC could be used as a demo for prospective client(s). ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine); 
                contentSb.Append(Environment.NewLine);
                contentSb.Append("The details of order are here as under:");
                contentSb.Append(Environment.NewLine);
                contentSb.Append($"Shipped Order = {request.OrderId}, ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append($"For Product = {request.ProductId}, ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append($"Quantity = {request.Quantity}, ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append($"To Address = {request.Address}");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(" Thanks & Regards,");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(" Divyesh Bhartiya");

                SendBody body = new SendBody
                {
                    Sender = "bhartiya.divyesh@gmail.com",
                    Subject = "Payment Processor and Notification Service Implementation Using gRPC.",
                    BodyText = contentSb.ToString()
                };

                Response res = await _mailClient.SendAsync(body);
                Console.WriteLine(res.Message);

                return new SendOrderReply
                {
                    Ok = true
                };
            }
            catch
            {
                return new SendOrderReply
                {
                    Ok = false
                };
            }
        }
    }
}

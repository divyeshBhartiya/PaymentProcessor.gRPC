using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NotificationService;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                contentSb.Append("Hi, ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append("The Payment of your Order has been processed. Thanks for using your debit card. ");
                contentSb.Append("We have saved the card details for future references. ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append("The details of your Order are here as under:");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append($"The Order# {request.OrderId}, is Shipped and Delivered ");
                contentSb.Append($"to {request.Address} at ");
                contentSb.Append($"{string.Format("{0:hh:mm:ss tt}", DateTime.Now)}.");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append($"The box contains {request.Quantity} set(s) of {request.ProductId}. We hope you love it !!! :-) ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append("This mail is sent by Payment Processing Application, a POC which involves gRPC communication between Microservices over Http/2 protocol. ");
                contentSb.Append("The idea is to create a payment processing scenario for a particular order and ");
                contentSb.Append("trigger shippment process for the same. ");
                contentSb.Append("Once the payment is processed the shipping service would ship the order and ");
                contentSb.Append("notification service would notify the user.");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append("The POC implements the mailing service to send notification to the user. " +
                    "And hence you are reading this mail.");
                contentSb.Append(Environment.NewLine);
                contentSb.Append("This POC could be used for demos or for further research on gRPC. ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append("For more details on gRPC, please visit: https://grpc.io/. ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append("For demo/walkthrough, please contact the developer on weekdays. ");
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append(Environment.NewLine);
                contentSb.Append("Thanks & Regards,");
                contentSb.Append(Environment.NewLine);
                contentSb.Append("Divyesh Bhartiya");

                SendBody body = new SendBody
                {
                    Sender = "bhartiya.divyesh@gmail.com",
                    Subject = "Payment Processed",
                    BodyText = contentSb.ToString()
                };

                await _mailClient.SendAsync(body);

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

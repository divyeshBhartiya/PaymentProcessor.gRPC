using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProcessorAPI.Dto;
using PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentProcessorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentProcessController : ControllerBase
    {
        // POST api/<PaymentProcess>
        [HttpPost]
        public async Task<string> ProcessPayment([FromBody] PaymentRequestDto payment)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var handler = new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(60),
                EnableMultipleHttp2Connections = true
            };

            var channel = GrpcChannel.ForAddress("https://localhost:5002", new GrpcChannelOptions
            {
                HttpHandler = handler
            });

            var paymentClient = new PaymentSvc.PaymentSvcClient(channel);

            StringBuilder sb = new StringBuilder();

            sb.Append("Calling methods from gRPC Client for Payment Processor");
            sb.Append(Environment.NewLine);
            var reply = await paymentClient.MakePaymentAsync(new MakePaymentRequest
            {
                ProductId = payment.ProductId,
                Quantity = payment.Quantity,
                Address = payment.Address,
            });
            sb.Append($"Made payment with transactionId: {reply.TransactionId}");
            sb.Append(Environment.NewLine);

            using var statusReplies = paymentClient.GetPaymentStatus(new GetPaymentStatusRequest() { TransactionId = reply.TransactionId });
            while (await statusReplies.ResponseStream.MoveNext())
            {
                var statusReply = statusReplies.ResponseStream.Current.Status;
                sb.Append($"Payment status: {statusReply}");
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}

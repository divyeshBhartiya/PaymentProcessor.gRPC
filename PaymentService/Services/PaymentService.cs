using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ShippingService;

namespace PaymentService
{
    public class PaymentService : PaymentSvc.PaymentSvcBase
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly ProductShipment.ProductShipmentClient _shippings;
        public PaymentService(ILogger<PaymentService> logger, ProductShipment.ProductShipmentClient shippings)
        {
            _logger = logger;
            _shippings = shippings;
        }

        public override async Task<MakePaymentReply> MakePayment(MakePaymentRequest request, ServerCallContext context)
        {
            var transactionId = Guid.NewGuid().ToString();
            _logger.LogInformation($"Make payment {transactionId}");
            Console.WriteLine($"Make payment {transactionId}");

            Console.WriteLine($"Shipping Order for transaction =" + transactionId + "," +
                $"/n ProductId =" + request.ProductId + "," +
                $"/n Quantity =" + request.Quantity + "," +
                $"/n Address =" + request.Address);

            await _shippings.SendOrderAsync(new SendOrderRequest
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Address = request.Address,
                OrderId = new Guid("A3CDAD9BF7FA4699AE38CB68278089FB").ToString()
            });

            return (new MakePaymentReply
            {
                TransactionId = transactionId
            });
        }

        public override async Task GetPaymentStatus(GetPaymentStatusRequest request, IServerStreamWriter<GetPaymentStatusResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"Payment Status Requested for Transaction: {0}", request.TransactionId);

            await Task.Delay(100);
            await responseStream.WriteAsync(
                new GetPaymentStatusResponse { Status = "Created" });
            await Task.Delay(100);
            await responseStream.WriteAsync(
                new GetPaymentStatusResponse { Status = "Validated" });
            await Task.Delay(100);
            await responseStream.WriteAsync(
                new GetPaymentStatusResponse { Status = "Accepted" });
        }
    }
}

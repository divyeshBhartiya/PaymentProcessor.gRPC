using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentProcessorAPI.Dto
{
    public class PaymentRequestDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Address { get; set; }
    }
}

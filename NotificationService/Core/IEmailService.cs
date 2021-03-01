using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Core
{
    public interface IEmailService
    {
       Task<Response> SendAsync(string from, RepeatedField<string> to, string subject, string html);
    }
}

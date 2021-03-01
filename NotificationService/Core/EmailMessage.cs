using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationService.Core
{
    public class EmailMessage
    {
        public MailboxAddress Sender { get; set; }
        public MailboxAddress Reciever { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}

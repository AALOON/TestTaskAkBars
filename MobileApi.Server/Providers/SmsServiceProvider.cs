using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MobileApi.Server.Providers
{
    public class SmsMessage
    {
        public SmsMessage(string destination, string subject, string body)
        {
            Destination = destination;
            Subject = subject;
            Body = body;
        }

        public string Destination { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class SmsServiceProvider : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var smsMessage = new SmsMessage(message.Subject, message.Body, message.Destination);

            Action<object> sendSms = (object obj) =>
            {
                SmsMessage sms = (SmsMessage) obj;
                //TODO: Send SMS actual code
                Thread.Sleep(50);
            };

            return Task.Factory.StartNew(sendSms, smsMessage);
        }


    }
}
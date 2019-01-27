using Microsoft.Azure.WebJobs;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionWorkflow
{
    public class SendMailActivity
    {
        [FunctionName(Workflow.SendMail)]
        public static string Run(
           [ActivityTrigger] RegisterUserDto registerUserDto,
           [OrchestrationClient] DurableOrchestrationClient starter,
           [SendGrid(ApiKey = "SendGridApiKey")] ICollector<SendGridMessage> messageCollector)
        {
            string toMail = registerUserDto.EmailAddress;
            string fromMail = Utility.GetEnvironmentVariable("SendGridFrom");
            var message = new SendGridMessage { Subject = $"KLAB 2019 #1" };
            message.AddTo(toMail);
            Content content = new Content
            {
                Type = "text/html",
                Value = $@"La richiesta di registrazione per l'utente {registerUserDto.Name} è stata preso in carico
                <br><a href='{Utility.GetEnvironmentVariable("PublicUrl")}/ApproveUser?userIdConfirmation={registerUserDto.IdConfirmationMail}'>Conferma registrazione</a>"
            };

            message.From = new EmailAddress(fromMail);
            message.AddContents(new[] { content }.ToList());
            messageCollector.Add(message);

            return Guid.NewGuid().ToString("N");
        }
    }
}

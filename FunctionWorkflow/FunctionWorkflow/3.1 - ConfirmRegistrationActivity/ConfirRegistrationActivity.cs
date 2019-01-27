using Microsoft.Azure.WebJobs;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionWorkflow
{
    public class ConfirRegistrationActivity
    {
        [FunctionName(Workflow.ConfirmRegistration)]
        public static string Run(
           [ActivityTrigger] ConfirmRegistrationDto confirmDto,
           [OrchestrationClient] DurableOrchestrationClient starter,
           [SendGrid(ApiKey = "SendGridApiKey")] ICollector<SendGridMessage> messageCollector)
        {
            string toMail = confirmDto.CurrentUser.EmailAddress;
            string fromMail = Utility.GetEnvironmentVariable("SendGridFrom");
            var message = new SendGridMessage { Subject = $"KLAB 2019 #1 - Confirm" };
            message.AddTo(toMail);
            string esito = confirmDto.Esito ? "Approvata" : "Rifiutata";
            Content content = new Content
            {
                Type = "text/html",
                Value = $@"La richiesta di registrazione per l'utente {confirmDto.CurrentUser.Name} è stata <b>{esito}</b>!"
            };

            message.From = new EmailAddress(fromMail);
            message.AddContents(new[] { content }.ToList());
            messageCollector.Add(message);

            return Guid.NewGuid().ToString("N");
        }
    }
}

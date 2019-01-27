using Microsoft.Azure.WebJobs;
using System;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Threading.Tasks;

namespace FunctionWorkflow
{
    public static class SendSMSActivity
    {
        [FunctionName(Workflow.SendSMS)]
        public static async Task<string> Run(
           [ActivityTrigger] RegisterUserDto registerUserDto,
           [TwilioSms(AccountSidSetting = "TwilioAccountSid",
                      AuthTokenSetting = "TwilioAuthToken")]
            IAsyncCollector<CreateMessageOptions> message,
            Microsoft.Extensions.Logging.ILogger logger)
        {
            string currentInstance = Guid.NewGuid().ToString("N");
            await message.AddAsync(new CreateMessageOptions(new PhoneNumber(registerUserDto.PhoneNumber))
            {
                Body = $"La registrazione per l'utente: {registerUserDto.Username} è preso in carico. Conferma la mail",
                From = new PhoneNumber(Utility.GetEnvironmentVariable("TwilioPhoneNumberBuy"))
            });
            await message.FlushAsync();

            return currentInstance;
        }
    }
}

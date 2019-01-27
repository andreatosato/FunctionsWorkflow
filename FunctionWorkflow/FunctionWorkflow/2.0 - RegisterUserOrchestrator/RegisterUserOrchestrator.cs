

using Microsoft.Azure.WebJobs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionWorkflow
{
    public class RegisterUserOrchestrator
    {
        [FunctionName("RegisterUserOrchestrator")]
        public async Task Run([OrchestrationTrigger] DurableOrchestrationContext context)
        {
            RegisterUserDto registerData = context.GetInput<RegisterUserDto>();
            // Id dell'istanza da attendere.
            registerData.IdConfirmationMail = context.InstanceId;

            // Invia notifica registrazione via MAIL
            string mailInstance = await context.CallActivityWithRetryAsync<string>(
                Workflow.SendMail,
                new RetryOptions(TimeSpan.FromSeconds(5), 5),
                registerData);

            // Invia notifica registrazione via SMS
            string smsInstance = await context.CallActivityAsync<string>(
                Workflow.SendSMS,
                registerData);

            // Mi metto in attesa dell'evento di conferma
            await WaitEventFromMailAsync(context, registerData);
        }

        /// <summary>
        /// Logica di attesa del click sul link della mail
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task WaitEventFromMailAsync(DurableOrchestrationContext context, RegisterUserDto registerData)
        {
            string status = string.Empty;
            using (var timeoutCts = new CancellationTokenSource())
            {
                // Rimango in attesa per MASSIMO 5 ore.
                DateTime dueTime = context.CurrentUtcDateTime.AddHours(5);
                Task durableTimeout = context.CreateTimer(dueTime, timeoutCts.Token);

                Task<bool> approvalTask = context.WaitForExternalEvent<bool>(Workflow.WaitEvent);
                //Attendo un evento o un timer
                if (approvalTask == await Task.WhenAny(approvalTask, durableTimeout))
                {
                    timeoutCts.Cancel();
                    if (await approvalTask)
                    {
                        // Approvato
                        string mailInstance = await context.CallActivityWithRetryAsync<string>(
                            Workflow.ConfirmRegistration,
                            new RetryOptions(TimeSpan.FromSeconds(5), 5),
                            new ConfirmRegistrationDto { CurrentUser = registerData, Esito = true });
                    }
                }
                else
                {
                    timeoutCts.Cancel();
                    // Rifiutato
                    string mailInstance = await context.CallActivityWithRetryAsync<string>(
                        Workflow.ConfirmRegistration,
                        new RetryOptions(TimeSpan.FromSeconds(5), 5),
                        new ConfirmRegistrationDto { CurrentUser = registerData, Esito = false });
                }
            }
        }
    }
}

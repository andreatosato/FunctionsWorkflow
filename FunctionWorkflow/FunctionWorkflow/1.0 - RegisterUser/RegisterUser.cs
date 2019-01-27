using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FunctionWorkflow
{

    public class RegisterUser
    {
        [FunctionName("RegisterUser")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, methods: "post", Route = "RegistraUtente")] HttpRequestMessage req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger logger)
        {
            RegisterUserDto registerUserDto = await req.Content.ReadAsAsync<RegisterUserDto>();
            string instanceId = await starter.StartNewAsync(Workflow.RegisterUserOrchestrator, registerUserDto);

            // Verifica completamento lavoro...
            logger.LogInformation($"Inizio Orchestratore con ID = '{instanceId}'.");
            var res = starter.CreateCheckStatusResponse(req, instanceId);
            res.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromMinutes(10));
            return res;
        }
    }
}

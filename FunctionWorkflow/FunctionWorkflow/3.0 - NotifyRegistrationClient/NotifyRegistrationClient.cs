using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionWorkflow
{
    public class NotifyRegistrationClient
    {
        [FunctionName("ApprovaOrdine")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ApproveUser")]HttpRequest req,
            [OrchestrationClient] DurableOrchestrationClient starter,
            ILogger logger)
        {
            string userIdConfirmation = req.GetQueryParameterDictionary()
                .First(x => x.Key == "userIdConfirmation")
                .Value;

            logger.LogInformation($"Approva Ordine {userIdConfirmation}");

            await starter.RaiseEventAsync(userIdConfirmation, Workflow.WaitEvent, true);
            return new AcceptedResult();
        }
    }
}

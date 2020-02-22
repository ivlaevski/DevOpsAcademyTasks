using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using FunctionAppTest1.Functions;

namespace FunctionAppTest1
{
    public static class AddNewTask
    {
        [FunctionName("AddNewTask")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("Tasks", Connection = "TaskTableConnectionString")]  ICollector<Tasks> tableBinding,
            [ServiceBus("input", Connection = "QueueConnectionString")] ICollector<string> outputSbQueue,
            ClaimsPrincipal principal,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger AddNewTask function processed a request.");

            string requestBody, UserId, GivenName, SurName;;
            try
            {
                (requestBody, UserId, GivenName, SurName) = await SharedCode.ParseRequest(req, principal);
            }
            catch (Exception ex)
            {
                return (ActionResult)new BadRequestObjectResult(ex.Message);
            }

            NewTask n = JsonConvert.DeserializeObject<NewTask>(requestBody);
            if (n.QueueName.ToLower() == "my")
            {
                //store it localy
                tableBinding.Add(new Tasks
                {
                    CreatorName = GivenName + " " + SurName,
                    Done = null,
                    Added = DateTime.UtcNow,
                    PartitionKey = UserId,
                    RowKey = Guid.NewGuid().ToString(),
                    Text = n.Text
                });
                return (ActionResult)new OkObjectResult($"Task added");
            }
            else
            {
                //send message to queue
                QueueMessage m = new QueueMessage
                {
                    Added = DateTime.UtcNow,
                    CreatorName = GivenName + " " + SurName,
                    QueueName = n.QueueName,
                    Text = n.Text
                };
                outputSbQueue.Add(JsonConvert.SerializeObject(m));
                return (ActionResult)new OkObjectResult($"Task submitted");
            }
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using FunctionAppTest1.Functions;

namespace FunctionAppTest1
{
    public static class GetMyTasks
    {
        [FunctionName("GetMyTasks")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Tasks", Connection = "TaskTableConnectionString")]      JArray inputTable,
            [Table("Targets", Connection = "TaskTableConnectionString")]    JArray targetsTable,
            ClaimsPrincipal principal,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger GetMyTasks function processed a request.");
            try
            {
                string requestBody, UserId, GivenName, SurName; ;
                try
                {
                    (requestBody, UserId, GivenName, SurName) = await SharedCode.ParseRequest(req, principal);
                }
                catch (Exception ex)
                {
                    return (ActionResult)new BadRequestObjectResult(ex.Message);
                }

                MyTasksResponce resp = new MyTasksResponce
                {
                    Targets = new List<Targets>(),
                    Tasks = new List<Tasks>()
                };

                foreach (JToken jToken in inputTable)
                {
                    Tasks p = Newtonsoft.Json.JsonConvert.DeserializeObject<Tasks>(jToken.ToString());
                    if ((p.PartitionKey == UserId) || (p.PartitionKey == "external"))
                    {
                        resp.Tasks.Add(p);
                    }
                }

                resp.Targets.Add(new Targets
                {
                    PartitionKey = UserId,
                    QueueName = "my",
                    RowKey = "1",
                    TargetName = "to myself"
                });
                foreach (JToken jToken in targetsTable)
                {
                    Targets p = Newtonsoft.Json.JsonConvert.DeserializeObject<Targets>(jToken.ToString());
                    resp.Targets.Add(p);
                }
                resp.Targets.Add(new Targets
                {
                    PartitionKey = UserId,
                    QueueName = "all",
                    RowKey = "99999",
                    TargetName = "to all"
                });
                return (ActionResult)new OkObjectResult(JsonConvert.SerializeObject(resp));
            }
            catch(Exception ex)
            {
                return (ActionResult)new OkObjectResult($"Exception {ex.Message}");
            }
        }
    }
}

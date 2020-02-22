using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.Security.Claims;
using FunctionAppTest1.Functions;

namespace FunctionAppTest1
{
    public static class DeleteTask
    {
        [FunctionName("DeleteTask")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table("Tasks", Connection = "TaskTableConnectionString")]
             CloudTable outputTable,
            ClaimsPrincipal principal,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger DeleteTask function processed a request.");

            string requestBody, UserId, GivenName, SurName; ;
            try
            {
                (requestBody, UserId, GivenName, SurName) = await SharedCode.ParseRequest(req, principal);
            }
            catch (Exception ex)
            {
                return (ActionResult)new BadRequestObjectResult(ex.Message);
            }

            Tasks taskToUpdate = JsonConvert.DeserializeObject<Tasks>(requestBody);
            taskToUpdate.ETag = "*";

            if ((UserId.ToLower() != taskToUpdate.PartitionKey.ToLower()) && ("external" != taskToUpdate.PartitionKey.ToLower()))
            {
                return (ActionResult)new BadRequestObjectResult("Acccess denied");                
            }

            var operation = TableOperation.Delete(taskToUpdate);
            await outputTable.ExecuteAsync(operation);

            return (ActionResult)new OkObjectResult($"Task deleted");
        }
    }
}

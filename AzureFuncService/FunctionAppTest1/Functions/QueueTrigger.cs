using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace FunctionAppTest1
{
    public static class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        public static void Run(
            [ServiceBusTrigger(GlobalVariables.CreatorQueue, Connection = "QueueConnectionString")]string myQueueItem,
            [Table("Tasks", Connection = "TaskTableConnectionString")] ICollector<Tasks> tableBinding,
            ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            QueueMessage m = JsonConvert.DeserializeObject<QueueMessage>(myQueueItem);

            tableBinding.Add(new Tasks
            {
                CreatorName = m.CreatorName,
                Done = null,
                Added = m.Added,
                PartitionKey = "external",
                RowKey = Guid.NewGuid().ToString(),
                Text = m.Text
            });
        }
    }
}

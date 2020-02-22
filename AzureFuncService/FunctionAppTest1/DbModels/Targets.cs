namespace FunctionAppTest1
{
    using Microsoft.WindowsAzure.Storage.Table;

    public class Targets : TableEntity
    {
        public string TargetName { get; set; }
        public string QueueName { get; set; }
    }
}

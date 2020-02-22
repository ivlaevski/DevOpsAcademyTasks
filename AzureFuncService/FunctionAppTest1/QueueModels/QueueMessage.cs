namespace FunctionAppTest1
{
    using System;
    public class QueueMessage
    {
        public string CreatorName { get; set; }
        public string Text { get; set; }
        public DateTime Added { get; set; }
        public string QueueName { get; set; }
    }
}

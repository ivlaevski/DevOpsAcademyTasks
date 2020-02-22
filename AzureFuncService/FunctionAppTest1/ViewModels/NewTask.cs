using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionAppTest1
{
    public class NewTask
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("queue")]
        public string QueueName { get; set; }
    }
}

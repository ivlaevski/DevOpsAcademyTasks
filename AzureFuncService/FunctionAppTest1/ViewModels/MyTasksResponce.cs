namespace FunctionAppTest1
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class MyTasksResponce
    {
        [JsonProperty("t")]
        public List<Tasks> Tasks { get; set; }

        [JsonProperty("s")]
        public List<Targets> Targets { get; set; }
    }
}

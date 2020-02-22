namespace FunctionAppTest1
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System;

    public class Tasks : TableEntity
    {
        public string Text { get; set; }
        public DateTime Added { get; set; }
        public DateTime? Done { get; set; }
        public string CreatorName { get; set; }
    }
}

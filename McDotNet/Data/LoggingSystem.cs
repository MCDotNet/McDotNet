namespace McDotNet.Data
{
    public class LoggingSystem
    {
        public Log Client { get; set; }
        public Log Server { get; set; }
        public class Log
        {
            public Artifact File { get; set; }
            public string Argument { get; set; }
            public string Type { get; set; }
        }      
    }
}

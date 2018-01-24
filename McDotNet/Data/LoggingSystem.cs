using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public class LoggingSystem
    {
        public Log Client { get; set; }
        public Log Server { get; set; }
        public class Log
        {
            public Artifact File { get; set; }
        }
        public string Arguemnt { get; set; }
        public string Type { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public class Library
    {
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("downloads")] // Plural but it's only one object...
        public Download Download { get; set; }
        [Newtonsoft.Json.JsonProperty("extract")] 
        public ExtractInstructions ExtractRules { get; set; }
    }
}

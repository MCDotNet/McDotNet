using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace McDotNet.Data
{
    public class Classifer
    {
        [JsonProperty("native-windows")]
        public Artifact Artifact { get; set; }       
    }
}

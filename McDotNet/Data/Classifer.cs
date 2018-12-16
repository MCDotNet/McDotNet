using Newtonsoft.Json;

namespace McDotNet.Data
{
    public class Classifer
    {
        [JsonProperty("natives-windows")]
        public Artifact Artifact { get; set; }       
    }
}

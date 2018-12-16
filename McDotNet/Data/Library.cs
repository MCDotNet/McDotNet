namespace McDotNet.Data
{
    public class Library
    {
        public string Name { get; set; }
        [Newtonsoft.Json.JsonProperty("downloads")] // Plural but it's only one object...
        public Download Download { get; set; }
        [Newtonsoft.Json.JsonProperty("extract")] 
        public ExtractRules ExtractInstructions { get; set; }
    }
}

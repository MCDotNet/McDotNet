namespace McDotNet.Data
{
    public abstract class ArtifactBase
    {
        public long Size { get; set; }
        public string Sha1 { get; set; }
        public string Url { get; set; }
    }
}

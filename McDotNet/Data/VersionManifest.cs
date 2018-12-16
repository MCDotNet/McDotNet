namespace McDotNet.Data
{
    public class VersionManifest
    {
        public LatestVersions Latest { get; set; }
        public MinecraftVersion[] Versions { get; set; }
    }
}
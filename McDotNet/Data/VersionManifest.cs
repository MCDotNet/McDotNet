namespace McDotNet.Data
{
    public class VersionManifest
    {
        public LatestVersions Latest { get; set; }
        public ManifestVersion[] Versions { get; set; }
    }
}

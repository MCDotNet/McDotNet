namespace McDotNet.Data
{
    public enum VersionType
    {
        /// <summary>
        /// A Minecraft snapshot, is often named like "18w01".
        /// </summary>
        Snapshot,
        /// <summary>
        /// A Minecraft release, is often named like "1.12.2"
        /// </summary>
        Release,
        /// <summary>
        /// A Minecraft beta version, is often named like "b1.8.2"
        /// </summary>
        OldBeta,
        /// <summary>
        /// A Minecraft alpha version, is often named like "a1.0.14"
        /// </summary>
        OldAlpha
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public class VersionManifest
    {
        /// <summary>
        /// The latest versions names of Minecraft.
        /// </summary>
        public LatestVersion Latest { get; set; }

        /// <summary>
        /// Gets the latest snapshot of this manifest.
        /// </summary>
        /// <returns>The latest snapshot.</returns>
        public VersionDownload GetLatestSnapshot() => Versions.Where(v => v.VersionName == Latest.Snapshot).First();
        /// <summary>
        /// Gets the latest release of this manifest
        /// </summary>
        /// <returns>The latest release.</returns>
        public VersionDownload GetLatestRelease() => Versions.Where(v => v.VersionName == Latest.Release).First();
        /// <summary>
        /// All the versions in this manifest.
        /// </summary>
        public VersionDownload[] Versions { get; set; }

        /// <summary>
        /// A class for containing the latest versions data.
        /// </summary>
        public class LatestVersion
        {
            /// <summary>
            /// The latest snapshot's name.
            /// </summary>
            public string Snapshot { get; set; }
            /// <summary>
            /// The latest release's name.
            /// </summary>
            public string Release { get; set; }
        }
    }
}

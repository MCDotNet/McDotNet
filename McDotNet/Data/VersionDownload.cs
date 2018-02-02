using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    /// <summary>
    /// A Minecraft version available for downloading
    /// </summary>
    public class VersionDownload
    {
        /// <summary>
        /// The name of the version, example : 1.12.1, or 18w02...
        /// </summary>
        [Newtonsoft.Json.JsonProperty("id")]
        public string VersionName { get; set; }
        /// <summary>
        /// The type of the version, release, snapshot, beta or alpha.
        /// </summary>
        public VersionType Type { get; set; }
        /// <summary>
        /// this idk tbh
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// The date when that version got released.
        /// </summary>
        public DateTime ReleaseTime { get; set; }
        /// <summary>
        /// The URL in which you can download the .jar file. Oh and hi cth, i'm sure you need this.
        /// </summary>
        public string Url { get; set; }
    }
}

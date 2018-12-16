using System;
using System.Collections.Generic;

namespace McDotNet.Data
{
    public class MinecraftVersion
    {
        public List<Library> Libraries { get; set; } = new List<Library>();
        public IndexArtifact AssetIndex { get; set; }
        public JarDownload Downloads { get; set; }
        public string MainClass { get; set; }
        public string MinecraftArguments { get; set; }
        public DateTime ReleaseTime { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; } // TODO : make this an enum
        public string Id { get; set; } // version thing
        public string Assets { get; set; } // version thing
        public LoggingSystem Logging { get; set; } // log thingy
        public string Url { get; set; }
    }
}

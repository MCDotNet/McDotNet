using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public class ManifestVersion
    {
        public string Id { get; set; } // lol noob version
        public string Type { get; set; } // TODO : make this an enum
        public string Url { get; set; } // lol noob url
        public DateTime Time { get; set; } // lol noob time
        public DateTime ReleaseTime { get; set; } // lol noob reles tim
    }
}

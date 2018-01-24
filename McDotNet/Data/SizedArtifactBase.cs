using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public abstract class SizedArtifactBase : ArtifactBase
    {
        public long TotalSize { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public class SizedArtifact : Artifact // Inherits from Artifact.
    {
        public long TotalSize { get; set; }
    }
}

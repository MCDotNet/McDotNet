using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.Data
{
    public class Download
    {
        public virtual Artifact Artifact { get; set; }
        /// <summary>
        /// Can be null ! Used for downloading native files
        /// </summary>
        public Artifact NativeFile { get; set; }
        [Newtonsoft.Json.JsonConstructor]
        public Download(Classifer classifiers)
        {
            NativeFile = classifiers?.Artifact ?? null;
        }
        public ConcurrentBag<string> GetUrlsToDownload()
        {
            var concurrent = new ConcurrentBag<string>();
            if (Artifact != null)
            {
                concurrent.Add(Artifact.Url);
            }
            if (NativeFile != null)
            {
                concurrent.Add(NativeFile.Url);
            }
            return concurrent;
        }
    }
}

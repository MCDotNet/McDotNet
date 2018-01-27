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
        public ConcurrentBag<Url> GetUrlsToDownload()
        {
            var concurrent = new ConcurrentBag<Url>();
            if (Artifact != null)
            {
                concurrent.Add(new Url(Artifact.Url, false));
            }
            if (NativeFile != null)
            {
                concurrent.Add(new Url(NativeFile.Url,false));
            }
            return concurrent;
        }
        public struct Url
        {
            public bool IsNative { get; set; }
            public string DownloadUrl { get; set; }
            public override string ToString() => DownloadUrl;
            public Url(string url, bool native)
            {
                IsNative = native;
                DownloadUrl = url;
            }
            public static implicit operator string(Url u) => u.ToString();
        }
    }
}

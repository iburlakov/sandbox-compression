using System;
using System.IO;
using System.Net;

using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace sandboxcompression
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var request = WebRequest.Create("http://www.google.com") as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "application/xml; charset=\"utf-8\"";
            request.UserAgent = "sandbox-compression";        
 
            request.Headers.Add("accept-encoding", "gzip, deflate");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                var stream = response.GetResponseStream();

                Console.WriteLine(response.Headers.ToString());

                Stream decompressionStream = null;
                if (response.ContentEncoding.IndexOf("gzip", StringComparison.InvariantCultureIgnoreCase) >=
                     0)
                {
                    decompressionStream = new GZipInputStream(stream);
                    Console.WriteLine("GZIP");
                }
                else if (response.ContentEncoding.IndexOf("deflate", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    decompressionStream = new InflaterInputStream(stream);
                    Console.WriteLine("DEFLATE");
                }
                else
                {
                    Console.WriteLine("PLAIN");
                }

                var reader = new StreamReader(decompressionStream ?? stream);

                Console.WriteLine(reader.ReadToEnd().Substring(0, 20));
            }

            Console.ReadKey();
        }
    }
}

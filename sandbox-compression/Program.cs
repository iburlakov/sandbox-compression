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
            var delimiter = new string('-', 10);

            var url = "https://google.com";
            Console.WriteLine("{0} sending request to {1} {0}", delimiter, url);
            var request = WebRequest.Create(url) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "application/xml; charset=\"utf-8\"";
            request.UserAgent = "sandbox-compression";        
 
            request.Headers.Add("accept-encoding", "gzip, deflate");

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                var stream = response.GetResponseStream();

                Console.WriteLine("{0} HEADERS {0}", delimiter);
                Console.WriteLine(response.Headers.ToString().Trim());

                string encoding = "NO";
                Stream decompressionStream = null;
                if (response.ContentEncoding.IndexOf("gzip", StringComparison.InvariantCultureIgnoreCase) >=
                     0)
                {
                    decompressionStream = new GZipInputStream(stream);
                    encoding = "GZIP";
                }
                else if (response.ContentEncoding.IndexOf("deflate", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    decompressionStream = new InflaterInputStream(stream);
                    encoding = "DEFLATE";
                }

                Console.WriteLine("{0} {1} {0}", delimiter, encoding);

                var reader = new StreamReader(decompressionStream ?? stream);

                Console.WriteLine("{0} First 20 chars of response body {0}", delimiter);

                Console.WriteLine(reader.ReadToEnd().Substring(0, 20));
            }

            Console.ReadKey();
        }
    }
}

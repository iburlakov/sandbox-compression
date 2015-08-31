using System;
using System.IO;
using System.Net;

namespace sandboxcompression
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var request = WebRequest.Create("http://google.com") as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "application/xml; charset=\"utf-8\"";
            request.UserAgent = "sandbox-compression";        
 
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;


            using (var response = request.GetResponse() as HttpWebResponse)
            {
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);

                Console.WriteLine(response.Headers.ToString());

                Console.WriteLine(reader.ReadToEnd().Substring(0, 20));
            }

            Console.ReadKey();
        }
    }
}

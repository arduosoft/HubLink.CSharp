using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HubLink.Clients.Helpers
{
    public static class LogHelper
    {
        public static string RequestGET(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }


        public static string DoRequest(string url, string postData)
        {

            return DoRequest(url, postData, "POST");
        }

        public static string DoRequest(string url, string postData,string requestMethod)
        {
            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            // Set the Method property of the request to POST.
            request.Method = requestMethod;

            if (requestMethod == "POST")
            {
                // Create POST data and convert it to a byte array.
                //request.Credentials = new NetworkCredential("admin", "123456");
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
             
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
            }
            request.ContentType = "application/json";
            request.Accept = "application/json";

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream2 = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream2);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream2.Close();
            response.Close();

            return responseFromServer;
        }
    }


}

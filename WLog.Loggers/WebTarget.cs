﻿using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NLog.WebLog
{
        [Target("WebTarget")]
        public  class WebTarget : TargetWithLayout
        {
            public class LogMessage
            {
                public DateTime SourceDate { get; set; }
                public string Message { get; set; }
                public string Level { get; set; }
                public string ApplicationKey { get; set; }
            }

             
            public WebTarget()
            {
               
                this.Destination = "localhost";
            }

            public WebTarget(String Destination)
            {
                this.Destination = Destination;

            }

            [RequiredParameter]
            public string Destination { get; set; }

            [RequiredParameter]
            public string ApplicationKey { get; set; }

            protected override void Write(LogEventInfo logEvent)
            {
                string logMessage = this.Layout.Render(logEvent);
                
                LogMessage entry = new LogMessage();
                entry.Message = logMessage;
                entry.SourceDate = DateTime.Now;
                entry.ApplicationKey = ApplicationKey;
                entry.Level = logEvent.Level.ToString();
               
               

                DoRequest(Destination, JsonConvert.SerializeObject(new LogMessage[] {entry }));
             
            }


            public static void DoRequest(string url, string postData)
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(url);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
                //request.Credentials = new NetworkCredential("admin", "123456");
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
           //     Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
               // Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }

            
        } 
    
}

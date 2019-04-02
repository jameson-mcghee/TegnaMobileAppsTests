﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Configuration;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace BrowserStackIntegration
{
    public class GoogleAnalytics : BrowserStackIntegrationImplementation
    {
        public GoogleAnalytics(string profile, string device) : base(profile, device){}

        public static string UserName => Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME") ??
                           ConfigurationManager.AppSettings.Get("user");
        public static string AccessKey => Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY") ??
                        ConfigurationManager.AppSettings.Get("key");
                
        public static async Task<dynamic> GetSessionResponseAsync(string url)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var authenticationByteArray = Encoding.ASCII.GetBytes($"{UserName}:{AccessKey}");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authenticationByteArray));
                    httpClient.Timeout = new TimeSpan(0, 0, 0, 0, 20000);
                    var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                    var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject(responseString);
                }
            }
            catch (Exception ex)
            {
                string message = $"GetResponseAsync - Error getting response from " + url + ".Ex: " + ex;
                Debug.WriteLine(message);
                //Debug.ReadLine();
                Console.WriteLine(message);
                throw;
            }
        }
                
        public async Task GetNetworkLogs()
        {

            // Get the response.
            dynamic responseSession = await GetSessionResponseAsync("https://api-cloud.browserstack.com/app-automate/builds/0085f3aa4b2d24d533398891ebf5b05f97fe8286/sessions.json?limit=1");

            if (responseSession.Count == 0)
            {
                throw new Exception("No session information in the API response.");
            }

            dynamic sessionLog = responseSession[0].automation_session;
            dynamic sessionId = sessionLog.hashed_id;

            if (string.IsNullOrEmpty(sessionId?.ToString()))
            {
                throw new Exception("No Session ID in the API response.");
            }

            dynamic responseNetwork = await GetSessionResponseAsync("https://api-cloud.browserstack.com/app-automate/builds/<build-id>/sessions/" + sessionId + "/networklogs");
            if (responseNetwork.Count == 0)
            {
                throw new Exception("No Network Log data in the API response.");
            }
                       
        }

    }
}

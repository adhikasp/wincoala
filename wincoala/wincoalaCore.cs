using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace wincoala
{
    public class WincoalaCore
    {
        private HttpClient apiClient;

        public WincoalaCore()
        {
            this.apiClient = new HttpClient(
                new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                });
            this.apiClient.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.apiClient.BaseAddress = new Uri("https://webservices.coala.io/");
        }

        public List<BearMetadata> getBearList()
        {
            // API call
            HttpResponseMessage response = this.apiClient.GetAsync("list/bears").Result;
            response.EnsureSuccessStatusCode();
            string resultAsString = response.Content.ReadAsStringAsync().Result;
                
            // Convert JSON -> Dict<bearName, BearMetadata> -> List<BearMetadata>
            Dictionary<String, BearMetadata> bearsData = 
                JsonConvert.DeserializeObject<Dictionary<String, BearMetadata>>(resultAsString);
            List<BearMetadata> result = new List<BearMetadata>();
            foreach(KeyValuePair<String, BearMetadata> bear in bearsData) {
                BearMetadata newBear = bear.Value;
                newBear.name = bear.Key;
                result.Add(newBear);
            }

            return result;
        }

        public List<Result> lintOnline(LintRequest request)
        {
            // API call
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            String test = JsonConvert.SerializeObject(request);
            Trace.WriteLine(test);
            HttpResponseMessage response = this.apiClient.PostAsync("editor/", content).Result;
            response.EnsureSuccessStatusCode();
            string resultAsString = response.Content.ReadAsStringAsync().Result;

            // Convert JSON to List<Result>
            LintResponse bearsData =
                JsonConvert.DeserializeObject<LintResponse>(resultAsString);
            // "default" is the default section name used in coala result
            List<Result> result = bearsData.results["default"];

            return result;
        }
    }
}

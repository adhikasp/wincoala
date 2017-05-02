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
        private static WincoalaCore instance; 

        private Persistence persistenceLayer;
        private HttpClient apiClient;

        /**
         * Singleton
         */
        public static WincoalaCore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WincoalaCore();
                }
                return instance;
            }
        }

        public WincoalaCore()
        {
            this.persistenceLayer = Persistence.Instance;

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

        /// <summary>
        /// Get all bear.
        /// </summary>
        /// <param name="forceSync">Bypass cache and force API call</param>
        /// <returns>
        /// List of bear
        /// </returns>
        public List<BearMetadata> getBearList(Boolean forceSync = false)
        {
            if (!forceSync)
            {
                List<BearMetadata> cachedBear = persistenceLayer.getAllBear();
                if (cachedBear.Count > 0)
                {
                    Trace.WriteLine("Get bear data from cache.");
                    return cachedBear;
                }
            }
            Trace.WriteLine("Get bear data from internet.");

            // API call
            // TODO handle connection error
            HttpResponseMessage response = this.apiClient.GetAsync("list/bears").Result;
            response.EnsureSuccessStatusCode();
            string resultAsString = response.Content.ReadAsStringAsync().Result;
                
            // Convert JSON -> Dict<bearName, BearMetadata> -> List<BearMetadata>
            Dictionary<String, BearListResponse> bearsData =
                JsonConvert.DeserializeObject<Dictionary<String, BearListResponse>>(resultAsString);
            List<BearMetadata> result = new List<BearMetadata>();
            foreach (KeyValuePair<String, BearListResponse> bear in bearsData)
            {
                BearMetadata newBear = new BearMetadata();
                newBear.Name = bear.Key;
                newBear.Description = bear.Value.desc;
                newBear.setLanguagesFromList(bear.Value.languages);
                result.Add(newBear);
            }

            // Cache bear list to DB
            persistenceLayer.saveBear(result);
            return result;
        }

        public List<Result> lintOnline(LintRequest request)
        {
            // API call
            Trace.WriteLine("editor API call");
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            String test = JsonConvert.SerializeObject(request);
            Trace.WriteLine(test);
            HttpResponseMessage response = this.apiClient.PostAsync("editor/", content).Result;
            response.EnsureSuccessStatusCode();
            string resultAsString = response.Content.ReadAsStringAsync().Result;
            Trace.WriteLine("Result: " + resultAsString);

            // Convert JSON to List<Result>
            LintResponse bearsData =
                JsonConvert.DeserializeObject<LintResponse>(resultAsString);
            // "default" is the default section name used in coala result
            List<Result> results = bearsData.results["default"];

            injectResultWithSnippet(request, ref results);
            return results;
        }

        private void injectResultWithSnippet(LintRequest request, ref List<Result> results)
        {
            string[] lines = request.file_data.Split(new String[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (Result result in results)
            {
                result.snippets = new List<String>();
                foreach (SourceRange sourceRange in result.affected_code)
                {
                    // The code itself
                    String snippet = lines[sourceRange.start.line - 1];
                    // Pointer on the wronged column, only if the bear support it
                    if (sourceRange.start.column != -1)
                    {
                        snippet += "\r\n" + new String(' ', sourceRange.start.column - 1) + "^";
                    }
                    result.snippets.Add(snippet);
                }
            }
        }
    }
}

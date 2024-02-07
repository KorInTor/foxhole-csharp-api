using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FoxholeSimpleAPI.WarData;
using FoxholeSimpleAPI.WarData.MapItems;
using System.Data;

namespace FoxholeSimpleAPI
{
    public class WarApiClient
    {
        private static readonly HttpClient client = new HttpClient();
        private Dictionary<string, string> etags = new Dictionary<string, string>();

        public List<string> namesList = new();

        public delegate Task MapDataHandler(string mapName, string data);
        public event MapDataHandler MapChanged;


        private string currentRootPoint;
        
        private Dictionary<string, string> rootEndpoints = new Dictionary<string, string>(){
            {"Able","https://war-service-live.foxholeservices.com/api"},
            {"Charlie","https://war-service-live-3.foxholeservices.com/api"}
        };

        public WarApiClient(string shardName)
        {
            try
            {
                this.currentRootPoint = rootEndpoints[shardName];
            }
            catch
            {
                throw new ArgumentException("Wrong shard name!");
            }
            LoadEntityTags();
        }

        /// <summary>
        /// Returns true if connection with API was succesfull.
        /// </summary>
        /// <returns>true if was succesfull false otherwise.</returns>
        public async Task<bool> IsConnectionSuccesfull()
        {
            using HttpResponseMessage response = await client.GetAsync(currentRootPoint + @"/worldconquest/war");
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Loads <see cref="etags"/> from file or creates new().
        /// </summary>
        public void LoadEntityTags()
        {
            etags = WorldConquestSerializer.LoadEtagsFromFile();

            etags ??= new Dictionary<string, string>();
        }

        /// <summary>
        /// Init <see cref="etags"/> with values from server. Used when no data is presented or new War starts.
        /// </summary>
        public async Task InitializeEntityTags()
        {
            etags = new Dictionary<string, string>();

            string responseString = string.Empty;
            try
            {
                responseString = await client.GetStringAsync(currentRootPoint + "/worldconquest/maps");
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                if (ex.Message.Contains("503"))
                {
                    Console.WriteLine("Сервера временно не доступны будут загруженны старые данные");
                }
            }
            //TODO: More exception catch.

            namesList = JsonConvert.DeserializeObject<List<string>>(responseString);

            foreach (string name in namesList)
            {
                etags.Add(name, string.Empty);
            }

            WorldConquestSerializer.SaveToFile(etags);
        }


        /// <summary>
        /// Requests WarAPI for data, if data is updated then Invoke <see cref="MapChanged"/>.
        /// </summary>
        /// <param name="mapName">Map name for which data will be requested</param>
        public async Task GetDynamicDataAsync(string mapName)
        {
            string url = $"{currentRootPoint}/worldconquest/maps/{mapName}/dynamic/public";

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (!string.IsNullOrEmpty(etags[mapName]))
            {
                request.Headers.IfNoneMatch.ParseAdd(etags[mapName]);
            }

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotModified)
            {
                //Console.WriteLine($"Nothing changed for {mapName}");
                return;
            }

            etags[mapName] = response.Headers.ETag?.Tag;

            WorldConquestSerializer.SaveToFile(etags);

            var data = await response.Content.ReadAsStringAsync();

            MapChanged?.Invoke(mapName, data);
        }

        /// <summary>
        /// Returns data of current war on chosen shard.
        /// </summary>
        /// <returns>String that can be serialized to <see cref="War"/>.</returns>
        public async Task<string> GetWardataAsync()
        {
            string url = $"{currentRootPoint}/worldconquest/war";

            using HttpResponseMessage response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Returns value of War report for given map.
        /// </summary>
        /// <param name="mapName">Map Name for wich get war report.</param>
        /// <returns>War Report.</returns>
        public async Task<string> GetWarReportAsync(string mapName)
        {
            string url = $"{currentRootPoint}/worldconquest/warReport/{mapName}";

            using HttpResponseMessage response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
            // [X] TODO: Получение репорта для карты.
        }

        /// <summary>
        /// Returns value of static data for given map.
        /// </summary>
        /// <param name="mapName">Map Name that data for will be getted.</param>
        /// <returns>String that can be serialized to <see cref="Map"/>.</returns>
        public async Task<string> GetStaticDataAsync(string mapName)
        {
            string url = $"{currentRootPoint}/worldconquest/maps/{mapName}/static";

            using HttpResponseMessage response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
            //[X] TODO: Получение статичных данных на которые наслаиваются данные из dynamic.
        }

        /// <summary>
        /// Returns Mapnames of current war. 
        /// </summary>
        /// <returns>MapNames.</returns>
        public async Task<string> GetMapNamesAsync()
        {
            string url = $"{currentRootPoint}/worldconquest/maps";

            using HttpResponseMessage response = await client.GetAsync(url);

            return await response.Content.ReadAsStringAsync();
        }
    }
}

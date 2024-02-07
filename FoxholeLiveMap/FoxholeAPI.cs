using FoxholeSimpleAPI.WarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FoxholeSimpleAPI
{
    public class FoxholeAPI
    {

        public delegate void ConnectionHandler(string connectionMessage);
        public event ConnectionHandler ConnectionHappend;

        private string currentShard;

        private WarApiClient client;

        public WorldConquest WorldConquestData { get; private set; }

        private string[] shards = { "Able", "Charlie" };

        public FoxholeAPI(string shardName)
        {
            if (!shards.Contains(shardName))
                throw new ArgumentException("Wrong shard name!");

            currentShard = shardName;
            WorldConquestSerializer.Shard = currentShard;
            client = new WarApiClient(currentShard);
        }

        public async Task Start()
        {
            AssertDirectoriesAreCreated();
            Console.WriteLine("Directories are created");
            await AssertWarIsUpToDate();
            Console.WriteLine("War is up to date.");
            await InitializeUpdateListenerAsync();
        }

        private async Task AssertWarIsUpToDate()
        {
            WorldConquestData = WorldConquestSerializer.IsDataAlreadySaved() ? WorldConquestSerializer.LoadFromFile() : new WorldConquest();

            //Данные о самой войне достоверны?
            if (WorldConquestData.War?.WarId
                != JsonConvert.DeserializeObject<War>(await client.GetWardataAsync()).WarId)
            {
                await client.InitializeEntityTags();
                await FillStaticData();
                WorldConquestSerializer.SaveToFile(WorldConquestData);
            }
        }

        public async Task<bool> IsConnectionSuccesfull()
        {
            return await client.IsConnectionSuccesfull();
        }

        public async Task InitializeUpdateListenerAsync()
        {
            client.MapChanged += HandleDynamicChange;
            client.LoadEntityTags();

            Console.WriteLine("Entity tags loaded");

            foreach (Map map in WorldConquestData.Maps)
            {
                WorldConquestData.MapNames.Add(map.Name);
            }

            Task.Run(StartListen);
        }

        private async Task FillStaticData()
        {
            WorldConquestData.War = JsonConvert.DeserializeObject<War>(await client.GetWardataAsync());

            //Получение информации о названии карт в действующей войне.
            foreach (string name in JsonConvert.DeserializeObject<List<string>>(await client.GetMapNamesAsync()))
            {
                Map map = new();
                map.Name = name;
                WorldConquestData.Maps.Add(map);
            }

            //Получение статичной информации с карт в действующей войне.
            for (int i = 0; i < WorldConquestData.Maps.Count; i++)
            {
                string data = await client.GetStaticDataAsync(WorldConquestData.Maps[i].Name);

                Map map = JsonConvert.DeserializeObject<Map>(data);

                WorldConquestData.Maps[i].mapTextItems = map.mapTextItems;
                WorldConquestData.Maps[i].regionId = map.regionId;

                if (WorldConquestData.Maps[i].Name == "" || WorldConquestData.Maps[i].Name == null)
                {
                    throw new Exception("Map name are blank???");
                }
            }
        }

        private async Task StartListen()
        {
            do
            {
                Console.WriteLine("Making new query;" + "\n");

                foreach (string name in WorldConquestData.MapNames)
                {
                    client.GetDynamicDataAsync(name);
                }
                await Task.Delay(5000);
            } while (true);
        }

        private void AssertDirectoriesAreCreated()
        {
            string wdir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // Working Directory;

            foreach (string shard in shards)
            {
                Directory.CreateDirectory(Path.Combine(wdir, shard, "WorldConquestData", "Maps"));
            }
        }

        private async Task HandleDynamicChange(string mapName, string? data)
        {
            Console.WriteLine($"{mapName} is changed!!!");

            if (data == null)
            {
                throw new NullReferenceException("Переданные данные пусты?");
            }
            
            Map newData = JsonConvert.DeserializeObject<Map>(data);

            newData.Name = mapName;

            Map savedData = WorldConquestData.Maps.FirstOrDefault(x => x.Name == mapName);
            //При стандартной работе, не может быть null
            savedData.mapItems = newData.mapItems;

            WorldConquestSerializer.SaveToFile(savedData);
        }
    }
}

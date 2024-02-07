using FoxholeSimpleAPI.WarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.WebRequestMethods;

namespace FoxholeSimpleAPI
{
    /// <summary>
    /// Methods for current War data serialization.
    /// </summary>
    public static class WorldConquestSerializer
    {
        private static string Filename { get; set; }

        private static string _shard;
        
        private static string WorldConquestDataDirectory { get; set; }

        private static string MapsDirectory
        {
            get 
            { 
                return WorldConquestDataDirectory + @"\Maps"; 
            }
        }

        public static string Shard
        {
            get 
            {
                return _shard;
            }

            set 
            {
                _shard = value;
                WorldConquestDataDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                    @$"\{Shard}\WorldConquestData";
            }
        }

        private static void CreateDirectory()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Filename));
        }

        public static void SaveToFile(WorldConquest data)
        {
            Filename = WorldConquestDataDirectory + @"\war.json";
            try
            {
                CreateDirectory();
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(Filename))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    ((JsonTextWriter)writer).Formatting = Formatting.Indented; //Форматирование в удобный для чтения формат.

                    serializer.Serialize(writer, data.War);
                }
                foreach (Map map in data.Maps)
                {
                    SaveToFile(map);
                }
            }
            catch
            {
                throw new Exception($"An error occurred while saving data to a file.");
            }
           
        }

        public static void SaveToFile(Map map)
        {
            Filename = MapsDirectory + $@"\{map.Name}.json";
            try
            {
                CreateDirectory();
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sw = new StreamWriter(Filename))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    ((JsonTextWriter)writer).Formatting = Formatting.Indented; //Форматирование в удобный для чтения формат.

                    serializer.Serialize(writer, map);
                }
            }
            catch
            {
                throw new Exception($"An error occurred while saving data to a file.");
            }
        }

        public static WorldConquest LoadFromFile()
        {
            Filename = WorldConquestDataDirectory + @"\war.json";

            WorldConquest data = new WorldConquest();
            data.Maps = LoadMapsFromFile();
            data.War = LoadWarDataFromFile();
            return data;
        }


        private static List<Map> LoadMapsFromFile()
        {
            string[] files = Directory.GetFiles(MapsDirectory, "*.json");

            List<Map> maps = new List<Map>();

            try
            {
                CreateDirectory();
                foreach (string file in files)
                {
                    JsonSerializer serializer = new JsonSerializer();
                    using (StreamReader sr = new StreamReader(file))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        maps.Add(serializer.Deserialize<Map>(reader));
                    }
                }
            }
            catch
            {
                return new List<Map>();
            }
            return maps;
        }

        public static void SaveToFile(Dictionary<string, string> etags)
        {
            Filename = WorldConquestDataDirectory + @"\WarApi_ETags.json";
            try
            {
                CreateDirectory();
                JsonSerializer serializer = new();
                using (StreamWriter sw = new StreamWriter(Filename))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    ((JsonTextWriter)writer).Formatting = Formatting.Indented; //Форматирование в удобный для чтения формат.

                    serializer.Serialize(writer, etags);
                }
            }
            catch
            {
                throw new Exception($"An error occurred while saving data to a file.");
            }
        }

        public static War LoadWarDataFromFile()
        {
            Filename = WorldConquestDataDirectory + @"\war.json";

            War data;

            try
            {
                CreateDirectory();
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sr = new StreamReader(Filename))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    data = serializer.Deserialize<War>(reader);
                }
                if (data.WarId == null)
                {
                    throw new Exception("War Data is Corruoted returning new");
                }
                return data;
            }
            catch
            {
                return new War();
            }
        }

        public static Dictionary<string, string>? LoadEtagsFromFile()
        {
            Filename = WorldConquestDataDirectory + @"\WarApi_ETags.json";
            Dictionary<string, string>? etags = null;

            try
            {
                CreateDirectory();

                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sr = new StreamReader(Filename))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    etags = serializer.Deserialize<Dictionary<string, string>>(reader);
                }
            }
            catch
            {
                return null;
            }
            return etags;
        }

        public static void SaveToFile(List<string> mapNames)
        {
            Filename = WorldConquestDataDirectory + @"\Map_Names.json";
            try
            {
                CreateDirectory();
                JsonSerializer serializer = new();
                using (StreamWriter sw = new StreamWriter(Filename))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    ((JsonTextWriter)writer).Formatting = Formatting.Indented; //Форматирование в удобный для чтения формат.

                    serializer.Serialize(writer, mapNames);
                }
            }
            catch
            {
                throw new Exception($"An error occurred while saving data to a file.");
            }
        }

        public static List<string>? LoadMapNamesFromFile()
        {
            Filename = WorldConquestDataDirectory + @"\Map_Names.json";

            List<string>? data = new();

            try
            {
                CreateDirectory();
                JsonSerializer serializer = new JsonSerializer();
                using (StreamReader sr = new StreamReader(Filename))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    data = serializer.Deserialize<List<string>>(reader);
                }
            }
            catch
            {
                return data;
            }
            return data;
        }

        public static bool IsDataAlreadySaved()
        {
            if (System.IO.File.Exists(WorldConquestDataDirectory + @"\war.json"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

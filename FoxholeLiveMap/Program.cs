using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FoxholeSimpleAPI.WarData;
using Newtonsoft.Json;

namespace FoxholeSimpleAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            FoxholeAPI api = new FoxholeAPI("Able");
            if (await api.IsConnectionSuccesfull())
            {
                await api.Start();
            }
            else
            {
                Console.WriteLine("Соединение не получилось проверьте вашу сеть!");
            }
            Console.ReadKey();
        }
    }
}

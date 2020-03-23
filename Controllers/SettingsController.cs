using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WorkWithFarmacy.DB;
using WorkWithFarmacy.Models;

namespace WorkWithFarmacy.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private static DbContextOptionsBuilder<CatalogContext> optionBuilder = new DbContextOptionsBuilder<CatalogContext>();
        private static DbContextOptions<CatalogContext> option = optionBuilder.UseNpgsql(@"Server = 127.0.0.1; User Id = postgres; Password = timur; Port = 5432; Database = PharmDb;").Options;
        private const string APP_PATH = "http://sso.asna.cloud:6000/connect/token";
        public const string client_id = "a51db5a7-4b1d-4a4d-983b-dbeaa7ab80b5";
        private static string token;
        private const string since = "";
        private const string PUTFULLSTOCK_PATH = "https://api.asna.cloud/v5/stores/" + client_id + "/stocks";


        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Settings()
        {
            
            _logger.LogInformation("Test Message");
            return View();
        }

        static Dictionary<string, string> GetTokenDictionary(string client_id, string client_secret)
        {
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
                    new KeyValuePair<string, string>( "client_id", client_id ),
                    new KeyValuePair<string, string> ( "client_secret", client_secret ),
                    new KeyValuePair<string, string>( "X-ClientId", "manuscript" )
                };
            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(APP_PATH + "/Token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }

        static HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }

        public async void PostFullStock()
        {
            List<Stock> list;
            List<FullStockFiltered> listFiltered = new List<FullStockFiltered>();
            List<PostStock> fullStockList = new List<PostStock>();
            string client_id = "a51db5a7-4b1d-4a4d-983b-dbeaa7ab80b5";
            string client_secret = "8rU2zvHA";

            Dictionary<string, string> tokenDictionary = GetTokenDictionary(client_id, client_secret);
            token = tokenDictionary["access_token"];


            using (CatalogContext db = new CatalogContext(option))
            {
                db.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
               // db.Database.ExecuteSqlRaw(@"CREATE VIEW View_FullStock AS Select PrtId, Nnt, Qnt, SupInn, Nds, PrcOptNds, PrcRet From 'Stocks'");                
                var fullStock = db.FullStock.ToList();

                //var stock = (from c in db.Stocks where c.Qnt > 0 select c);
                //list = stock.ToList();
                foreach (var c in fullStock)
                {
                   // PostStock obj = new PostStock(c.PrtId, c.Nnt, c.Qnt, c.SupInn, c.Nds, c.PrcOptNds, c.PrcRet);
                    //FullStockFiltered obj = new FullStockFiltered(c.PrtId, c.Nnt, c.Qnt, c.SupInn, c.Nds, c.PrcOptNds, c.PrcRet);
                    //listFiltered.Add(obj);
                  //  fullStockList.Add(obj);
                }

            }
            


            FullStockListAndDate StockList = new FullStockListAndDate(DateTime.UtcNow, listFiltered);     
       
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore // ignore null values
            });          

            try
            {
                using (var client = CreateClient(token))
                {
                    string JsonStock = JsonConvert.SerializeObject(StockList, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                    var responce = await client.PostAsync(PUTFULLSTOCK_PATH, new StringContent(JsonStock, Encoding.UTF8, "application/json"));
                    System.Diagnostics.Debug.WriteLine(responce + "-------------------responce-----------------------");
                }
            }
            catch
            {
                if (Response.StatusCode == 401)
                {
                    tokenDictionary = GetTokenDictionary(client_id, client_secret);
                    token = tokenDictionary["access_token"];
                }
                if (Response.StatusCode == 429)
                {
                    System.Diagnostics.Debug.WriteLine("Sleep for 30 sec wating for token");
                    Thread.Sleep(30000);
                    tokenDictionary = GetTokenDictionary(client_id, client_secret);
                    token = tokenDictionary["access_token"];
                }
                if (Response.StatusCode == 400)
                {
                    System.Diagnostics.Debug.WriteLine("Bad Request");
                    throw new Exception("Bad Request 400");
                }
            }            
        }
    }
}
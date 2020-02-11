using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorkWithFarmacy.Models;

namespace WorkWithFarmacy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HttpClient client = new HttpClient();
        public StringContent httpContent = new StringContent("application/json");
        private const string APP_PATH = "http://sso.asna.cloud:6000/connect/token";
        private const string STORE_PATH = "https://api.asna.cloud/v5/references/stores";
        public const string client_id = "D82BA4CD-6F5A-46A5-92AD-FBBEA56AAE40";
        private const string PREORDER_PATH = "https://api.asna.cloud/v5/stores/"+client_id+"/preorders";
        private static string token;
        //public List<FarmacySettings> FarmacyList;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home(List<FarmacySettings> list)
        {
            //List<FarmacySettings> list = new List<FarmacySettings>();     

            //list.Add(new FarmacySettings() { Name = "OOO KO-KO-KO" });
            //list.RemoveRange(1, 19190);
            return View();
        }

        public IActionResult Preorder(List<Preorder> list)
        {
            List<Preorder> list1 = new List<Preorder>();
            //list1 = list;            

            //list1.Add(new Preorder() { Nnt = 1111111 });
            //list1.Add(new Preorder() { Nnt = 1111112 });
            //list1.Add(new Preorder() { Nnt = 1111113 });
            PerformRequest();

            return View(list);
        }
        public  IActionResult Index()
        {                       
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // получение токена
        static Dictionary<string, string> GetTokenDictionary(string client_id, string client_secret)
        {
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "client_credentials" ),
                    new KeyValuePair<string, string>( "client_id", client_id ),
                    new KeyValuePair<string, string> ( "client_secret", client_secret )
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

        // обращаемся по маршруту api/values 
        static async Task<List<Preorder>> GetValues(string token)
        {
            using (var client = CreateClient(token))
            {
                var streamTaskA = client.GetStreamAsync(PREORDER_PATH);
                var repositories = await System.Text.Json.JsonSerializer.DeserializeAsync<List<Preorder>>(await streamTaskA);

                 return repositories;
            }
        }

        // создаем http-клиента с токеном 
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

        public async Task PerformRequest()
        {
            string client_id = "D82BA4CD-6F5A-46A5-92AD-FBBEA56AAE40";
            string client_secret = "g0XoL4lw";

            Dictionary<string, string> tokenDictionary = GetTokenDictionary(client_id, client_secret);
            token = tokenDictionary["access_token"];

            var FarmacyList = await GetValues(token);
            Preorder(FarmacyList);
        }



    }
}

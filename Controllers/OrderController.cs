using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorkWithFarmacy.DB;
using WorkWithFarmacy.Models;

namespace WorkWithFarmacy.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string APP_PATH = "http://sso.asna.cloud:6000/connect/token";
        public const string client_id = "D82BA4CD-6F5A-46A5-92AD-FBBEA56AAE40";
        private static string token;
        private const string GETORDERS_PATH = "https://api.asna.cloud/v5/stores/" + client_id + "/orders_exchanger?since=2019-11-20";
        public PutOrderToSite order_filter = new PutOrderToSite();
        
        public async Task<ViewResult> Orders()
        {
            var list = await GetOrders();
            return View(list);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // получение токена
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

        static async Task<PutOrderToSite> GetValuesOrder(string token)
        {
            using (var client = CreateClient(token))
            {
               // var streamTaskA = client.GetStreamAsync(GETORDERS_PATH);
                var streamTaskA = await client.GetStringAsync(GETORDERS_PATH);
                var repositories =  System.Text.Json.JsonSerializer.Deserialize<PutOrderToSite>(streamTaskA);
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

        public async Task<PutOrderToSite> GetOrders()
        {
            var optionBuilder = new DbContextOptionsBuilder<CatalogContext>();
            var option = optionBuilder.UseNpgsql(@"Server = 127.0.0.1; User Id = postgres; Password = timur; Port = 5432; Database = PharmDb;")
                           .Options;
            string client_id = "D82BA4CD-6F5A-46A5-92AD-FBBEA56AAE40";
            string client_secret = "g0XoL4lw";

            Dictionary<string, string> tokenDictionary = GetTokenDictionary(client_id, client_secret);
            token = tokenDictionary["access_token"];

            var OrdersList = await GetValuesOrder(token);
            int countHeaders = 0;
            int countRows = 0;
            int countStatuses = 0;

            using (CatalogContext db = new CatalogContext(option))
            {
                for (int i = 0; i < OrdersList.statuses.Count; i++)
                {
                    if (OrdersList.statuses[i].Status == 100)
                    {
                        countStatuses++;
                        for (int j = 0; j < OrdersList.headers.Count; j++)
                        {

                            if (OrdersList.statuses[i].OrderId == OrdersList.headers[j].OrderId)
                            {
                                db.OrderHeader.Add(OrdersList.headers[j]);
                                db.OrderStatus.Add(OrdersList.statuses[i]);
                                countHeaders++;
                            }
                        }
                        for (int k = 0; k < OrdersList.rows.Count; k++)
                        {
                            if (OrdersList.statuses[i].OrderId == OrdersList.rows[k].OrderId)
                            {
                                db.OrderRows.Add(OrdersList.rows[k]);                                
                                countRows++;
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
            System.Diagnostics.Debug.WriteLine(countHeaders + " " + "=================================================================");
            System.Diagnostics.Debug.WriteLine(countRows + " " + "=================================================================");
            System.Diagnostics.Debug.WriteLine(countStatuses + " " + "=================================================================");
            
            return OrdersList;
        }
    }
}
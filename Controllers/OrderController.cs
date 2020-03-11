﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WorkWithFarmacy.DB;
using WorkWithFarmacy.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace WorkWithFarmacy.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string APP_PATH = "http://sso.asna.cloud:6000/connect/token";
        public const string client_id = "D82BA4CD-6F5A-46A5-92AD-FBBEA56AAE40";
        private static string token;
        private const string since = "";
        private const string GETORDERS_PATH = "https://api.asna.cloud/v5/stores/" + client_id + "/orders_exchanger?since="+ since + "";              
        public List<OrderRowToStore> listrowstosite = new List<OrderRowToStore>();
        public List<OrderStatusToStore> liststatusestosite = new List<OrderStatusToStore>();
        public PutOrderToSite toSite = new PutOrderToSite();
        private static DbContextOptionsBuilder<CatalogContext> optionBuilder = new DbContextOptionsBuilder<CatalogContext>();
        private static DbContextOptions<CatalogContext> option = optionBuilder.UseNpgsql(@"Server = 127.0.0.1; User Id = postgres; Password = timur; Port = 5432; Database = PharmDb;").Options;

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
            using (CatalogContext db = new CatalogContext(option))
            {                                
                var lastHeaderTs = db.OrderHeader.FromSqlRaw("SELECT Ts FROM OrderHeader ORDER BY Ts DESC LIMIT 1");
                var lastStatusTs = db.OrderStatus.FromSqlRaw("SELECT Ts FROM OrderStatus ORDER BY Ts DESC LIMIT 1");
                var lastRowTs = db.OrderRows.FromSqlRaw("SELECT Ts FROM OrderRow ORDER BY Ts DESC LIMIT 1");
                //var sinceTs = DateTime.Compare(lastHeaderTs, lastRowTs);
            }
                try
                {
                using (var client = CreateClient(token))
                {
                    // var streamTaskA = client.GetStreamAsync(GETORDERS_PATH);

                    var streamTaskA = await client.GetStringAsync(GETORDERS_PATH);
                    var repositories = System.Text.Json.JsonSerializer.Deserialize<PutOrderToSite>(streamTaskA);
                    return repositories;
                }
            }

            catch (NullReferenceException) { }
            return null;
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
            //try catch for getting token            
            string client_id = "D82BA4CD-6F5A-46A5-92AD-FBBEA56AAE40";
            string client_secret = "g0XoL4lw";    

            Dictionary<string, string> tokenDictionary = GetTokenDictionary(client_id, client_secret);
            token = tokenDictionary["access_token"];

            var OrdersList = await GetValuesOrder(token);
            System.Diagnostics.Debug.WriteLine(OrdersList + "---------------------array200 from api---------------------");
            if (OrdersList.rows.Count != 0)
            {
                using (CatalogContext db = new CatalogContext(option))
                {
                    for (int i = 0; i < OrdersList.statuses.Count; i++)
                    {
                        if (OrdersList.statuses[i].Status == 100)
                        {
                            OrderStatusToStore status200 = new OrderStatusToStore(Guid.NewGuid(), OrdersList.statuses[i].OrderId, OrdersList.statuses[i].RowId, DateTime.Today, OrdersList.statuses[i].RcDate, 200, DateTime.Now);
                            status200.StatusId = Guid.NewGuid();
                            db.OrderStatus.Add(status200);
                            for (int j = 0; j < OrdersList.headers.Count; j++)
                            {
                                if (OrdersList.statuses[i].OrderId == OrdersList.headers[j].OrderId)
                                {
                                    db.OrderHeader.Add(OrdersList.headers[j]);
                                    db.OrderStatus.Add(OrdersList.statuses[i]);
                                    liststatusestosite.Add(status200);
                                    toSite.statuses = liststatusestosite;
                                }
                            }
                            for (int k = 0; k < OrdersList.rows.Count; k++)
                            {
                                if (OrdersList.statuses[i].OrderId == OrdersList.rows[k].OrderId)
                                {
                                    db.OrderRows.Add(OrdersList.rows[k]);
                                    listrowstosite.Add(OrdersList.rows[k]);
                                    toSite.rows = listrowstosite;
                                }
                            }
                        }
                        else if (OrdersList.statuses[i].Status == 108)
                        {
                            for (int k = 0; k < OrdersList.rows.Count; k++)
                            {
                                if (OrdersList.statuses[i].OrderId == OrdersList.rows[k].OrderId)
                                {
                                    db.OrderRows.Remove(db.OrderRows.Single(a => a.OrderId == OrdersList.statuses[i].OrderId));                                   

                                    db.OrderRows.Add(OrdersList.rows[k]);

                                    db.OrderRows.Add(OrdersList.rows[k]);
                                    listrowstosite.Add(OrdersList.rows[k]);
                                    toSite.rows = listrowstosite;
                                    OrderStatusToStore status208 = new OrderStatusToStore(Guid.NewGuid(), OrdersList.statuses[i].OrderId, OrdersList.statuses[i].RowId, DateTime.Today, OrdersList.statuses[i].RcDate, 208, DateTime.Now);
                                    db.OrderStatus.Add(status208);
                                    liststatusestosite.Add(status208);
                                    toSite.statuses = liststatusestosite;
                                }
                            }
                            var status201 = BuildArrToSite(toSite);
                            PutOrsdersToSite(status201);
                            db.SaveChanges();
                        }
                        //else if (OrdersList.statuses[i].Status == 102)
                        //{
                        //    for (int k = 0; k < OrdersList.rows.Count; k++)
                        //    {
                        //        if (OrdersList.statuses[i].OrderId == OrdersList.rows[i].OrderId)
                        //        {

                        //        }
                        //    }
                        //}
                    }
                    
                    var array200 = BuildArrToSite(toSite);
                    PutOrsdersToSite(array200);
                    db.SaveChanges();
                }
            }
            System.Diagnostics.Debug.WriteLine(toSite);            
            return OrdersList;
        }

        public async void PutOrsdersToSite(ArrayOrdersToSite array)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore // ignore null values
            });

            string client_id = "D82BA4CD-6F5A-46A5-92AD-FBBEA56AAE40";
            string client_secret = "g0XoL4lw";
           
            try
            {
                using (var client = CreateClient(token))
                {
                    string cont = JsonConvert.SerializeObject(array,Formatting.Indented,new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                    System.Diagnostics.Debug.WriteLine(cont);
                    var responce = await client.PostAsync(GETORDERS_PATH, new StringContent(cont, Encoding.UTF8, "application/json"));
                    System.Diagnostics.Debug.WriteLine(responce + "---------------------200---------------------");

                }
            }
            catch
            {
                if (Response.StatusCode == 401)
                {
                    Dictionary<string, string> tokenDictionary = GetTokenDictionary(client_id, client_secret);
                    token = tokenDictionary["access_token"];
                }
                if (Response.StatusCode == 429)
                {
                    System.Diagnostics.Debug.WriteLine("Sleep for 30 sec wating for token");
                    Thread.Sleep(30000);
                    Dictionary<string, string> tokenDictionary = GetTokenDictionary(client_id, client_secret);
                    token = tokenDictionary["access_token"];
                }
            }
            finally
            {
                using (var client = CreateClient(token))
                {
                    string cont = JsonConvert.SerializeObject(array);
                    System.Diagnostics.Debug.WriteLine(cont);
                    var responce = await client.PostAsync(GETORDERS_PATH, new StringContent(cont, Encoding.UTF8, "application/json"));
                    System.Diagnostics.Debug.WriteLine(responce + "---------------------200---------------------");

                }
            }
        }
        
        public ArrayOrdersToSite BuildArrToSite(PutOrderToSite content)
        {
            List<SendOrderStatuses> listStatuses = new List<SendOrderStatuses>();
            List<SendOrderRows> listRows = new List<SendOrderRows>();            

            for (int i = 0; i < content.statuses.Count; i ++)
            {
                SendOrderStatuses obj = new SendOrderStatuses(content.statuses[i].StatusId, content.statuses[i].OrderId, content.statuses[i].StoreId, content.statuses[i].Date, content.statuses[i].Status);
                listStatuses.Add(obj);
            }

            for (int j = 0; j < content.rows.Count; j++)
            {
                SendOrderRows obj = new SendOrderRows(content.rows[j].RowId, content.rows[j].QntUnrsv);
                listRows.Add(obj);
            }

            ArrayOrdersToSite array = new ArrayOrdersToSite(listRows, listStatuses);
            System.Diagnostics.Debug.WriteLine(array + "---------------------array200 to api---------------------");

            return array;

        }

        public void GetRemains()
        {

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"remains.xlsx");
            Excel._Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.Worksheets[1];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= colCount; j++)
                {
                    //new line
                    if (j == 1)
                        Console.Write("\r\n");

                    //write the value to the console
                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j] != null)
                        Console.Write(xlRange.Cells[i, j].ToString() + "\t");

                    //add useful things here!   
                }
            }
        }

    }
}
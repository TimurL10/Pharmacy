using System;
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


namespace WorkWithFarmacy.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string APP_PATH = "http://sso.asna.cloud:6000/connect/token";
        public const string client_id = "a51db5a7-4b1d-4a4d-983b-dbeaa7ab80b5";
        private static string token;
        private static string since="";
        //private string GETORDERS_PATH = "https://api.asna.cloud/v5/stores/" + client_id + "/orders_exchanger?since=" + since + "";
        private string GETORDERS_PATH = "https://api.asna.cloud/v5/stores/" + client_id + "/orders_exchanger?since=2020-04-01";
        public PutOrderToSite toSite = new PutOrderToSite() { rows = new List<OrderRowToStore>(), statuses = new List<OrderStatusToStore>()};
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

        public async Task<PutOrderToSite> GetValuesOrder(string token)
        {            
                using (CatalogContext db = new CatalogContext(option))
                {
                    if (db.OrderHeader.Any() && db.OrderStatus.Any() && db.OrderStatus.Any())
                    {
                        var lastHeaderTs = (from c in db.OrderHeader select c.Ts).Max();
                        var lastStatusTs = (from c in db.OrderStatus select c.Ts).Max();
                        var lastRowTs = (from c in db.OrderRows select c.Ts).Max();

                        if (lastHeaderTs > lastRowTs)
                        {
                            if (lastHeaderTs > lastStatusTs)
                            {
                                since = lastHeaderTs.ToString();
                            }
                            else
                            {
                                since = lastStatusTs.ToString();
                            }
                        }
                        else
                        {
                            if (lastRowTs > lastStatusTs)
                            {
                                since = lastRowTs.ToString();
                            }
                            else
                            {
                                since = lastStatusTs.ToString();
                            }
                        }
                    }
                    else
                      since = "";

                }          
                using (var client = CreateClient(token))
                {
                    //string GETORDERS_PATH = "https://api.asna.cloud/v5/stores/" + client_id + "/orders_exchanger?since=" + since + "";                    
                    // var streamTaskA = client.GetStreamAsync(GETORDERS_PATH);
                    var streamTaskA = await client.GetStringAsync(GETORDERS_PATH);
                    if (streamTaskA.Length > 0)
                    {
                        var repositories = System.Text.Json.JsonSerializer.Deserialize<PutOrderToSite>(streamTaskA);
                        return repositories;
                    }                    
                }              
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
            string client_id = "a51db5a7-4b1d-4a4d-983b-dbeaa7ab80b5";
            string client_secret = "8rU2zvHA";    

            Dictionary<string, string> tokenDictionary = GetTokenDictionary(client_id, client_secret);
            token = tokenDictionary["access_token"];

            var OrdersList = await GetValuesOrder(token);
            if (OrdersList.rows.Count != 0)
            {
                using (CatalogContext db = new CatalogContext(option))
                {
                    try
                    {
                        for (int i = 0; i < OrdersList.statuses.Count; i++)
                        {
                            if (OrdersList.statuses[i].Status == 100)
                            {
                                for (int k = 0; k < OrdersList.statuses.Count; k++)
                                {
                                    if (OrdersList.statuses[i].OrderId == OrdersList.rows[k].OrderId)
                                    {
                                        var RowFromStock = (from c in db.Stocks where c.Nnt == OrdersList.rows[k].Nnt select c).FirstOrDefault();
                                        if (RowFromStock == null)
                                        {
                                            //202
                                            OrderStatusToStore status202 = new OrderStatusToStore(
                                           OrdersList.statuses[i].OrderId,
                                           OrdersList.statuses[i].RowId,
                                           OrdersList.statuses[i].StoreId,
                                           OrdersList.statuses[i].RcDate,
                                           202);
                                            toSite.statuses.Add(status202);
                                            break;
                                        }                                        
                                            if (RowFromStock.Qnt >= OrdersList.rows[k].Qnt)
                                        {
                                            //200                                            
                                            var reservedStock = new ReservedStock(RowFromStock, OrdersList.rows[k]); // create new obj put it to reserved stock
                                            db.ReservedStocks.Add(reservedStock);
                                            RowFromStock.Qnt -= OrdersList.rows[k].Qnt;
                                            db.Stocks.Update(RowFromStock);
                                            OrderStatusToStore status200 = new OrderStatusToStore(
                                            OrdersList.statuses[i].OrderId,
                                            OrdersList.statuses[i].RowId,
                                            OrdersList.statuses[i].StoreId,
                                            OrdersList.statuses[i].RcDate,
                                            200);
                                            db.OrderStatus.Add(status200);
                                            toSite.statuses.Add(status200);
                                            db.SaveChanges();
                                            break;
                                        }
                                        else if (RowFromStock.Qnt < OrdersList.rows[k].Qnt)
                                        {
                                            //201                                                
                                            var StockQntExist = OrdersList.rows[k].Qnt - RowFromStock.Qnt;
                                            OrdersList.rows[k].Qnt = RowFromStock.Qnt; // change existing qnt of row for save it in db
                                            var StockRow = (from c in db.Stocks where c.Nnt == OrdersList.rows[k].Nnt select c);
                                            var reservedStock = new ReservedStock(StockRow.First(), OrdersList.rows[k]); // create new obj put it to reserved stock
                                            db.ReservedStocks.Add(reservedStock);
                                            StockRow.First().Qnt = 0; // Zero Exist in stock now
                                            db.Stocks.Update(StockRow.First()); // updating Stock table
                                            OrdersList.rows[k].QntUnrsv = StockQntExist;
                                            OrderStatusToStore status201 = new OrderStatusToStore(
                                           OrdersList.statuses[i].OrderId,
                                           OrdersList.statuses[i].RowId,
                                           OrdersList.statuses[i].StoreId,
                                           OrdersList.statuses[i].RcDate,
                                           201);
                                            db.OrderStatus.Add(status201);
                                            toSite.statuses.Add(status201);
                                            toSite.rows.Add(OrdersList.rows[k]);
                                            db.SaveChanges();
                                            break;
                                        }
                                        else if (RowFromStock.Qnt <= 0)
                                        {
                                            //202
                                            OrderStatusToStore status202 = new OrderStatusToStore(
                                           OrdersList.statuses[i].OrderId,
                                           OrdersList.statuses[i].RowId,
                                           OrdersList.statuses[i].StoreId,
                                           OrdersList.statuses[i].RcDate,
                                           202);
                                            toSite.statuses.Add(status202);
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (OrdersList.statuses[i].Status == 108) // 108
                            {
                                for (int k = 0; k < OrdersList.rows.Count; k++)
                                {
                                    if (OrdersList.statuses[i].OrderId == OrdersList.rows[k].OrderId)
                                    {
                                        var reservedRow = (from c in db.ReservedStocks where c.RowId == OrdersList.rows[k].RowId select c).FirstOrDefault(); // take row from reserved table
                                        if (reservedRow == null)
                                        {
                                            //202
                                            OrderStatusToStore status202 = new OrderStatusToStore(
                                           OrdersList.statuses[i].OrderId,
                                           OrdersList.statuses[i].RowId,
                                           OrdersList.statuses[i].StoreId,
                                           OrdersList.statuses[i].RcDate,
                                           202);
                                            toSite.statuses.Add(status202);
                                            break;
                                        }                                        
                                        if (reservedRow.Qnt > OrdersList.rows[k].Qnt) // cus reduced qnt
                                        {
                                            var newQnt = reservedRow.Qnt - OrdersList.rows[k].Qnt;
                                            reservedRow.Qnt = OrdersList.rows[k].Qnt;
                                            db.ReservedStocks.Update(reservedRow);
                                            var stockObject = new Stock(reservedRow);
                                            db.Stocks.Add(stockObject); // adding row from reserve to stock
                                            OrderStatusToStore status201Edited = new OrderStatusToStore(
                                              OrdersList.statuses[i].OrderId,
                                              OrdersList.statuses[i].RowId,
                                              OrdersList.statuses[i].StoreId,
                                              OrdersList.statuses[i].RcDate,
                                              200);
                                            db.OrderStatus.Add(status201Edited);
                                            toSite.statuses.Add(status201Edited);
                                            db.SaveChanges();
                                            break;
                                        }
                                        else
                                        {
                                            var stockRow = (from c in db.Stocks where c.Nnt == OrdersList.rows[k].Nnt select c).FirstOrDefault();
                                            if (stockRow == null)
                                            {
                                                //202
                                                OrderStatusToStore status202 = new OrderStatusToStore(
                                               OrdersList.statuses[i].OrderId,
                                               OrdersList.statuses[i].RowId,
                                               OrdersList.statuses[i].StoreId,
                                               OrdersList.statuses[i].RcDate,
                                               202);
                                                toSite.statuses.Add(status202);
                                                break;
                                            }

                                            var needQnt = reservedRow.Qnt - OrdersList.rows[k].Qnt;
                                            stockRow.Qnt = +needQnt;
                                            if (stockRow.Qnt >= 0) // it enough nnt in stock
                                            {
                                                db.Stocks.Update(stockRow);
                                                reservedRow.Qnt = OrdersList.rows[k].Qnt;
                                                db.ReservedStocks.Update(reservedRow);
                                                toSite.rows.Add(OrdersList.rows[k]);
                                                OrderStatusToStore status201Edited = new OrderStatusToStore(
                                              OrdersList.statuses[i].OrderId,
                                              OrdersList.statuses[i].RowId,
                                              OrdersList.statuses[i].StoreId,
                                              OrdersList.statuses[i].RcDate,
                                              200);
                                                db.OrderStatus.Add(status201Edited);
                                                toSite.statuses.Add(status201Edited);
                                                db.SaveChanges();
                                                break;
                                            }
                                            else // not enough nnt in stock
                                            {
                                                OrdersList.rows[k].QntUnrsv = (stockRow.Qnt  +(stockRow.Qnt * -2));
                                                reservedRow.Qnt += (needQnt - stockRow.Qnt);
                                                db.ReservedStocks.Update(reservedRow);
                                                stockRow.Qnt = 0;
                                                db.Stocks.Update(stockRow);
                                                OrderStatusToStore status201Edited = new OrderStatusToStore(
                                             OrdersList.statuses[i].OrderId,
                                             OrdersList.statuses[i].RowId,
                                             OrdersList.statuses[i].StoreId,
                                             OrdersList.statuses[i].RcDate,
                                             201);
                                                db.OrderStatus.Add(status201Edited);
                                                toSite.statuses.Add(status201Edited);
                                                toSite.rows.Add(OrdersList.rows[k]);
                                                db.SaveChanges();
                                                break;
                                            }
                                        }
                                    }
                                }
                                var status201 = BuildArrToSite(toSite);
                                db.SaveChanges();
                            }
                            // Status 102
                            else if (OrdersList.statuses[i].Status == 102)
                            {
                                for (int k = 0; k < OrdersList.rows.Count; k++)
                                {
                                    if (OrdersList.statuses[i].OrderId == OrdersList.rows[i].OrderId)
                                    {
                                        var reservedRow = (from c in db.ReservedStocks where c.RowId == OrdersList.rows[i].RowId select c);
                                        db.Stocks.Add(new Stock(reservedRow.First()));
                                        db.ReservedStocks.Remove(reservedRow.First());
                                        db.SaveChanges();
                                        break;
                                    }
                                }
                            }
                            else if (OrdersList.statuses[i].Status == 111)
                            {
                                for (int j = 0; j < OrdersList.rows.Count; j++)
                                {
                                    if (OrdersList.statuses[i].OrderId == OrdersList.rows[j].OrderId)
                                    {
                                        var reservedRow = (from c in db.ReservedStocks where c.OrderId == OrdersList.statuses[i].OrderId select c).ToList();
                                        foreach (var c in reservedRow)
                                        {
                                            Stock ojectStock = new Stock(c);
                                            db.Stocks.Add(ojectStock);
                                            db.ReservedStocks.Remove(c);
                                            OrderStatusToStore status211 = new OrderStatusToStore(
                                                    OrdersList.statuses[i].OrderId,
                                                    OrdersList.statuses[i].RowId,
                                                    OrdersList.statuses[i].StoreId,
                                                    OrdersList.statuses[i].RcDate,
                                                    211);
                                            db.OrderStatus.Add(status211);
                                            toSite.statuses.Add(status211);
                                            db.SaveChanges();
                                        }
                                        break;
                                    }                                    
                                }
                            }
                        }
                    }
                    catch (ArgumentNullException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e);
                        Console.WriteLine(e);
                    }
                    var rowsAndStatusesArray = BuildArrToSite(toSite);
                    PutOrsdersToSite(rowsAndStatusesArray);                   
                }
                return OrdersList;
            }
            else
                return null;            
        }

        public async void PutOrsdersToSite(ArrayOrdersToSite array)
        {
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore // ignore null values
            });

            string client_id = "a51db5a7-4b1d-4a4d-983b-dbeaa7ab80b5";
            string client_secret = "8rU2zvHA";
           
            try
            {
                using (var client = CreateClient(token))
                {
                    string GETORDERS_PATH = "https://api.asna.cloud/v5/stores/" + client_id + "/orders_exchanger?since=" + since + "";
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
        }
        
        public ArrayOrdersToSite BuildArrToSite(PutOrderToSite content)
        {            
            List<SendOrderStatuses> listStatuses = new List<SendOrderStatuses>();
            List<SendOrderRows> listRows = new List<SendOrderRows>();

            try
            {
                for (int i = 0; i < content.statuses.Count; i++)
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

                return array;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);

            }
            return null;
        } 
        
        //public void RcDateReset()
        //{
        //    using (CatalogContext db = new CatalogContext(option))
        //    {
        //        var orders = db.OrderStatus.ToList();
        //        foreach (var c in orders)
        //        {

        //            if (c.OrderId)
        //        }
        //    }
        //}

    }
}
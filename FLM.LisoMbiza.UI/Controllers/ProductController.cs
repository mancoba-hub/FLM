using FLM.LisoMbiza.UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace FLM.LisoMbiza.UI.Controllers
{
    public class ProductController : Controller
    {
        #region Properties

        private readonly ILogger<ProductController> _productLogger;
        private readonly IConfiguration _configuration;
        private readonly string _productUrl = string.Empty;
        private HttpClient _httpClient;
        public ProductBranches ProductBranchList { get; set; }
        public List<Product> ProductList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productLogger"></param>
        /// <param name="configuration"></param>
        public ProductController(ILogger<ProductController> productLogger, IConfiguration configuration)
        {
            _productLogger = productLogger;
            _configuration = configuration;
            _productUrl = _configuration.GetValue<string>("Api:ProductEndpoint");
            _httpClient = GetHttpClient();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the products
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                ProductList = GetProducts();
                return View(ProductList);
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the product create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Actions the product create
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Product model)
        {
            try
            {
                ModelState.Remove("ID");
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("model.Id", "Please make sure that your details are correct and valid");
                    return View();
                }
                else
                {
                    var client = GetHttpClient();
                    string url = $"{_productUrl}create";
                    var response = SendObject<Product>(model, client, url, true);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            try
            {
                var client = GetHttpClient();
                var response = client.GetAsync($"{_productUrl}getById/?productId={id}").Result;
                var product = CreateResponse<Product>(response.Content);

                var branchUrl = _configuration.GetValue<string>("Api:BranchEndpoint");
                var branchResponse = _httpClient.GetAsync($"{branchUrl}all").Result;
                var branches = CreateResponse<List<Branch>>(branchResponse.Content);

                var productBranchList = _httpClient.GetAsync($"{_productUrl}getProductBranchById/?productId={id}").Result;
                var listProductBranch = CreateResponse<List<BranchProduct>>(productBranchList.Content);

                ProductBranchList = new ProductBranches
                {
                    ID = product.ID,
                    Name = product.Name,
                    WeightedItem = product.WeightedItem,
                    SuggestedSellingPrice = product.SuggestedSellingPrice,
                    Branches = PopulateProductBranches(branches, listProductBranch)
                };

                return View(ProductBranchList);
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;

            }
        }

        /// <summary>
        /// Actions the edit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, ProductBranches model)
        {
            try
            {
                ModelState.Remove("ID");
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("model.Id", "Please make sure that your details are correct and valid");
                    return View();
                }
                else
                {
                    //Update product
                    var updatedProduct = new Product
                    {
                        ID = model.ID,
                        Name = model.Name,
                        WeightedItem = model.WeightedItem,
                        SuggestedSellingPrice = model.SuggestedSellingPrice
                    };
                    var client = GetHttpClient();
                    string url = $"{_productUrl}update";
                    var response = SendObject<Product>(updatedProduct, client, url, false);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    //Update Branch Product
                    List<BranchProduct> branchProducts = new List<BranchProduct>();
                    foreach (var branchProduct in model.Branches.Where(x => x.IsChecked))
                    {
                        branchProducts.Add(new BranchProduct { BranchID = branchProduct.ID, ProductID = model.ID });
                    }
                    url = $"{_productUrl}deleteProductBranchById/?productId={id}";
                    response = client.DeleteAsync(url).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        url = $"{_productUrl}assign/";
                        response = SendObject<List<BranchProduct>>(branchProducts, client, url, false);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Assign(int id)
        {
            try
            {
                var response = _httpClient.GetAsync($"{_productUrl}getById/?productId={id}").Result;
                var product = CreateResponse<Product>(response.Content);

                var branchUrl = _configuration.GetValue<string>("Api:BranchEndpoint");
                var branchResponse = _httpClient.GetAsync($"{branchUrl}all").Result;
                var branches = CreateResponse<List<Branch>>(branchResponse.Content);

                var productBranchList = _httpClient.GetAsync($"{_productUrl}getProductBranchById/?productId={id}").Result;
                var listProductBranch = CreateResponse<List<BranchProduct>>(productBranchList.Content);

                ProductBranchList = new ProductBranches
                {
                    ID = product.ID,
                    Name = product.Name,
                    WeightedItem = product.WeightedItem,
                    SuggestedSellingPrice = product.SuggestedSellingPrice,
                    Branches = PopulateProductBranches(branches, listProductBranch)
                };

                return View(ProductBranchList);
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Assign(int id, string name, ProductBranches model)
        {
            try
            {
                //int id = model.ID;
                var mod = HttpContext.Request.Form["Name"];
                ModelState.Remove("ID");
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("model.Id", "Please make sure that your details are correct and valid");
                    return View();
                }
                else
                {
                    //Update Branch Product
                    List<BranchProduct> branchProducts = new List<BranchProduct>();
                    foreach (var branchProduct in model.Branches.Where(x => x.IsChecked))
                    {
                        branchProducts.Add(new BranchProduct { BranchID = branchProduct.ID, ProductID = id });
                    }
                    string url = $"{_productUrl}deleteProductBranchById/?productId={id}";
                    var response = _httpClient.DeleteAsync(url).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        url = $"{_productUrl}assign/";
                        response = SendObject<List<BranchProduct>>(branchProducts, _httpClient, url, false);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the product delete 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            try
            {
                var client = GetHttpClient();
                var data = new Dictionary<string, string>
                {
                    {"id",id.ToString()}
                };
                string url = $"{_productUrl}delete/?id={id}";
                var response = SendObject(data, client, url, false);
                var result = CreateResponseFull<Response>(response.Content);
                if (response.IsSuccessStatusCode)
                {
                    if (!result.IsSuccessful)
                    {
                        ModelState.AddModelError("ErrorMessage", result.ErrorMessage);
                        ViewBag.ErrorMessage = result.ErrorMessage;
                        ViewBag.Error = result.ErrorMessage;
                        ViewData["ErrorMessage"] = result.ErrorMessage;
                        Error();
                    }
                }
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Actions the product delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("model.Id", "Please make sure that your details are correct and valid");
                    return View();
                }
                else
                {
                    var client = GetHttpClient();
                    string url = $"{_productUrl}delete/?id={id}";
                    var response = client.DeleteAsync(url).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the xml export
        /// </summary>
        /// <returns></returns>
        public ActionResult XmlExport()
        {
            try
            {
                var products = GetProducts();
                string fileName = _configuration.GetValue<string>("Api:ProductDownloadPath");
                ExportHelper.ExportToXml<Product>(fileName, products);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the json export
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonExport()
        {
            try
            {
                var products = GetProducts();
                string fileName = _configuration.GetValue<string>("Api:ProductDownloadPath");
                ExportHelper.ExportToJson<Product>(fileName, products);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the csv export
        /// </summary>
        /// <returns></returns>
        public ActionResult CsvExport()
        {
            try
            {
                var products = GetProducts();
                string fileName = _configuration.GetValue<string>("Api:ProductDownloadPath");
                ExportHelper.ExportToCsv<Product>(fileName, products);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populates the product branches
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="branchProducts"></param>
        /// <returns></returns>
        private List<BranchProducts> PopulateProductBranches(List<Branch> branches, List<BranchProduct> branchProducts)
        {
            List<BranchProducts> productBranches = new List<BranchProducts>();
            foreach(var branch in branches)
            {
                var item = branchProducts.FirstOrDefault(b => b.BranchID == branch.ID);               
                if (item != default)
                {
                    productBranches.Add(new BranchProducts { ID = branch.ID, Name = branch.Name, IsChecked = true });
                    continue;
                }
                productBranches.Add(new BranchProducts { ID = branch.ID, Name = branch.Name, IsChecked = false });
            }
            return productBranches;
        }

        /// <summary>
        /// Gets the products
        /// </summary>
        /// <returns></returns>
        private List<Product> GetProducts()
        {
            try
            {
                var response = _httpClient.GetAsync($"{_productUrl}all").Result;
                var products = CreateResponse<List<Product>>(response.Content);
                return products;
            }
            catch (Exception e)
            {
                _productLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get the HttpClient instance
        /// </summary>
        /// <returns></returns>
        private HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_productUrl)
            };

            return client;
        }

        /// <summary>
        /// Creates the response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private T CreateResponse<T>(HttpContent response)
        {
            var stringData = response.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<Response<T>>(stringData).Data;
        }

        /// <summary>
        /// Creates the full response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private T CreateResponseFull<T>(HttpContent response)
        {
            var stringData = response.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<T>(stringData);
        }

        /// <summary>
        /// Sends the object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="url"></param>
        /// <param name="isPost"></param>
        /// <returns></returns>
        private HttpResponseMessage SendObject<T>(T data, HttpClient client, string url, bool isPost)
        {
            string json = JsonConvert.SerializeObject(data);
            StringContent stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            if (isPost)
            {
                return client.PostAsync(url, stringContent).GetAwaiter().GetResult();
            }
            return client.PutAsync(url, stringContent).GetAwaiter().GetResult();
        }

        #endregion
    }
}

using FLM.LisoMbiza.UI.Models;
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
    public class BranchReportController : Controller
    {
        #region Properties

        private readonly ILogger<BranchReportController> _reportLogger;
        private readonly IConfiguration _configuration;
        private readonly string _branchUrl = string.Empty;
        private readonly string _productUrl = string.Empty;
        private HttpClient _httpClient;

        private List<Product> ProductList { get; set; }

        private List<BranchProduct> BranchProductList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchReportController"/> class.
        /// </summary>
        /// <param name="reportLogger"></param>
        /// <param name="configuration"></param>
        public BranchReportController(ILogger<BranchReportController> reportLogger, IConfiguration configuration)
        {
            _reportLogger = reportLogger;
            _configuration = configuration;
            _branchUrl = _configuration.GetValue<string>("Api:BranchEndpoint");
            _productUrl = _configuration.GetValue<string>("Api:ProductEndpoint");
            _httpClient = GetHttpClient();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the page view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            BranchProductList = GetBranchProducts();
            ProductList = GetProducts();
            var reportBranches = GetReportBranches(BranchProductList, ProductList);

            return View(reportBranches);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the branches
        /// </summary>
        /// <returns></returns>
        private List<Branch> GetBranches()
        {
            try
            {
                var response = _httpClient.GetAsync($"{_branchUrl}all").Result;
                var branches = CreateResponse<List<Branch>>(response.Content);
                return branches;
            }
            catch (Exception e)
            {
                _reportLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
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
                _reportLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the branch products
        /// </summary>
        /// <returns></returns>
        private List<BranchProduct> GetBranchProducts()
        {
            try
            {
                var response = _httpClient.GetAsync($"{_productUrl}report").Result;
                var branchProducts = CreateResponse<List<BranchProduct>>(response.Content);
                return branchProducts;
            }
            catch (Exception e)
            {
                _reportLogger.Log(LogLevel.Error, e.ToString());
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
                BaseAddress = new Uri(_branchUrl)
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
        /// Gets the report branches
        /// </summary>
        /// <param name="branchProducts"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        private List<ReportBranch> GetReportBranches(List<BranchProduct> branchProducts, List<Product> products)
        {
            List<ReportBranch> reportBranches = new List<ReportBranch>();
            var branches = GetBranches();
            foreach(var branch in branches)
            {
                var tempList = branchProducts.Where(x => x.BranchID == branch.ID);
                foreach(var item in tempList)
                {
                    var product = products.FirstOrDefault(p => p.ID == item.ProductID);
                    if (product == null)
                        continue;

                    var reportBranch = reportBranches.FirstOrDefault(b => b.BranchId == item.BranchID);
                    if (reportBranch != default)
                    {
                        reportBranch.BranchProducts.Add(new ReportProduct { ProductId = item.ProductID, ProductName = product.Name });
                    }
                    else
                    {
                        reportBranches.Add(new ReportBranch
                        {
                            BranchId = branch.ID,
                            BranchName = branch.Name,
                            BranchProducts = new List<ReportProduct> {
                                new ReportProduct { 
                                    ProductId = item.ProductID, 
                                    ProductName = product.Name 
                                }
                            }
                        });
                    }                    
                }                
            }
            return reportBranches;
        }

        #endregion
    }
}

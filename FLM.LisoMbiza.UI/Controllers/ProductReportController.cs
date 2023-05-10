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
    public class ProductReportController : Controller
    {
        #region Properties

        private readonly ILogger<ProductReportController> _reportLogger;
        private readonly IConfiguration _configuration;
        private readonly string _branchUrl = string.Empty;
        private readonly string _productUrl = string.Empty;
        private HttpClient _httpClient;

        private List<Branch> BranchList { get; set; }

        private List<BranchProduct> BranchProductList { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductReportController"/> class.
        /// </summary>
        /// <param name="reportLogger"></param>
        /// <param name="configuration"></param>
        public ProductReportController(ILogger<ProductReportController> reportLogger, IConfiguration configuration)
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
            BranchList = GetBranches();
            var reportProducts = GetReportProducts(BranchProductList, BranchList);

            return View(reportProducts);
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
        /// Gets the report products
        /// </summary>
        /// <param name="branchProducts"></param>
        /// <param name="branches"></param>
        /// <returns></returns>
        private List<ReportProduct> GetReportProducts(List<BranchProduct> branchProducts, List<Branch> branches)
        {
            List<ReportProduct> reportProducts = new List<ReportProduct>();
            var products = GetProducts();
            foreach(var product in products)
            {
                var tempList = branchProducts.Where(x => x.ProductID == product.ID);
                foreach(var item in tempList)
                {
                    var branch = branches.FirstOrDefault(p => p.ID == item.BranchID);
                    if (branch == null)
                        continue;

                    var reportProduct = reportProducts.FirstOrDefault(b => b.ProductId == item.ProductID);
                    if (reportProduct != default)
                    {
                        reportProduct.ProductBranches.Add(new ReportBranch { BranchId = item.BranchID, BranchName = branch.Name });
                    }
                    else
                    {
                        reportProducts.Add(new ReportProduct
                        {
                            ProductId = product.ID,
                            ProductName = product.Name,
                            ProductBranches = new List<ReportBranch> {
                                new ReportBranch { 
                                    BranchId = item.BranchID, 
                                    BranchName = branch.Name 
                                }
                            }
                        });
                    }                    
                }                
            }
            return reportProducts;
        }

        #endregion
    }
}

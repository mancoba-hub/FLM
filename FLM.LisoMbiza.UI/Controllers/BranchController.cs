using FLM.LisoMbiza.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace FLM.LisoMbiza.UI.Controllers
{
    public class BranchController : Controller
    {
        #region Properties

        private readonly ILogger<BranchController> _branchLogger;
        private readonly IConfiguration _configuration;
        private readonly string _branchUrl = string.Empty;
        private HttpClient _httpClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="branchLogger"></param>
        /// <param name="configuration"></param>
        public BranchController(ILogger<BranchController> branchLogger, IConfiguration configuration)
        {
            _branchLogger = branchLogger;
            _configuration = configuration;
            _branchUrl = _configuration.GetValue<string>("Api:BranchEndpoint");
            _httpClient = GetHttpClient();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create the index page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var branches = GetBranches();
            return View(branches);
        }

        /// <summary>
        /// Actions the branch create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Actions the branch create
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Branch model)
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
                    string url = $"{_branchUrl}create";
                    var response = SendObject<Branch>(model, client, url, true);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _branchLogger.Log(LogLevel.Error, e.ToString());
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
                var response = client.GetAsync($"{_branchUrl}getById/?branchId={id}").Result;
                var branch = CreateResponse<Branch>(response.Content);
                return View(branch);
            }
            catch (Exception e)
            {
                _branchLogger.Log(LogLevel.Error, e.ToString());
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
        public ActionResult Edit(Guid id, IFormCollection collection, Branch model)
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
                    string url = $"{_branchUrl}update";
                    var response = SendObject<Branch>(model, client, url, false);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _branchLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actions the branch delete 
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
                string url = $"{_branchUrl}delete/?id={id}";
                var response = SendObject(data, client, url, false);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                _branchLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Actions the branch delete
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
                    string url = $"{_branchUrl}delete/?id={id}";
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
                _branchLogger.Log(LogLevel.Error, e.ToString());
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
                var products = GetBranches();
                string fileName = _configuration.GetValue<string>("Api:BranchDownloadPath");
                ExportHelper.ExportToXml<Branch>(fileName, products);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _branchLogger.Log(LogLevel.Error, e.ToString());
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
                var products = GetBranches();
                string fileName = _configuration.GetValue<string>("Api:BranchDownloadPath");
                ExportHelper.ExportToJson<Branch>(fileName, products);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _branchLogger.Log(LogLevel.Error, e.ToString());
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
                var products = GetBranches();
                string fileName = _configuration.GetValue<string>("Api:BranchDownloadPath");
                ExportHelper.ExportToCsv<Branch>(fileName, products);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _branchLogger.Log(LogLevel.Error, e.ToString());
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
                _branchLogger.Log(LogLevel.Error, e.ToString());
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

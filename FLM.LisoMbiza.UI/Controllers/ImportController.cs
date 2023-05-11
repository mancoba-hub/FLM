using FLM.LisoMbiza.UI.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FLM.LisoMbiza.UI.Controllers
{
    public class ImportController : Controller
    {
        private readonly ILogger<ImportController> _importLogger;
        private readonly IConfiguration _configuration;
        private readonly string _importUrl = string.Empty;
        private HttpClient _httpClient;

        public ImportController(ILogger<ImportController> importLogger, IConfiguration configuration)
        {
            _importLogger = importLogger;
            _configuration = configuration;
            _importUrl = _configuration.GetValue<string>("Api:ImportEndpoint");
            _httpClient = GetHttpClient();
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _importLogger.Log(LogLevel.Error, e.ToString());
                throw;
            }
        }

        public void ImportProductFile([FromForm] UploadForm uploadForm)
        {

        }
        
        public void ImportBranchFile(UploadForm uploadForm)
        {
            try
            {
                string url = $"{_importUrl}objectBranch/";
                SendFile(url, uploadForm.FileAttachment);
            }
            catch (Exception ex)
            {
                _importLogger.Log(LogLevel.Error, ex.ToString());
                throw;
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Private Methods

        /// <summary>
        /// Get the HttpClient instance
        /// </summary>
        /// <returns></returns>
        private HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(_importUrl)
            };

            return client;
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
        private HttpResponseMessage SendObject(string url, IFormFile formFile)
        {
            //var data = new Dictionary<string, string>
            //    {
            //        {"contentType", formFile.ContentType}
            //    };
            //string json = JsonConvert.SerializeObject(data);
            //StringContent stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var requestContent = new MultipartFormDataContent();
            StreamContent streamContent = new StreamContent(formFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            requestContent.Add(streamContent, "stream");
            return _httpClient.PostAsync(url, requestContent).GetAwaiter().GetResult();
        }

        private void SendJsonFile(string url, IFormFile formFile)
        {
            //var content = new MultipartFormDataContent();
            //var fileContent = new StreamContent(file.OpenReadStream());
            //fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

            //content.Add(fileContent, "file", file.FileName);

            //var jsonPayload = "that payload from the above sample";
            //var jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);
            //var jsonContent = new StreamContent(new MemoryStream(jsonBytes));
            //jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            //content.Add(jsonContent, "metadata", "metadata.json");

            //var response = await _httpClient.PostAsync("<The API URI>", content, cancellationToken);


            //using (var multipartFormContent = new MultipartFormDataContent())
            //{
            //    var fileContent = new StreamContent(formFile.OpenReadStream());
            //    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(formFile.ContentType);

            //    multipartFormContent.Add(fileContent, formFile.Name, formFile.FileName);

            //    var jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);
            //    var jsonContent = new StreamContent(new MemoryStream(jsonBytes));
            //    jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            //    var response = _httpClient.PostAsync(url, multipartFormContent).GetAwaiter().GetResult();
            //    response.EnsureSuccessStatusCode();
            //}
        }

        private void SendFile(string url, IFormFile formFile)
        {
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(formFile.OpenReadStream());
                fileContent.Headers.Add("Content-Type", "application/octet-stream");
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
                multipartFormContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                multipartFormContent.Add(fileContent, "fileUpload");

                var response = _httpClient.PostAsync(url, multipartFormContent).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
            }
        }

        #endregion
    }
}

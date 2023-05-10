using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FLM.LisoMbiza.WebUI.Pages
{
    public class ProductPageModel : PageModel
    {
        private readonly ILogger<ProductPageModel> _productLogger;
        private readonly HttpClient _httpClient;

        public ProductPageModel(ILogger<ProductPageModel> productLogger, HttpClient httpClient)
        {
            _productLogger = productLogger;
            _httpClient = httpClient;
        }

        public void OnGet()
        {
            var products = _httpClient.GetAsync("https://localhost:44327/api/product/all").GetAwaiter().GetResult();

        }
    }
}

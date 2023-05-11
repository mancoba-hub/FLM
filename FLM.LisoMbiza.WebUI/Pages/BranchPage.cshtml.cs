using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FLM.LisoMbiza.WebUI.Pages
{
    public class BranchPageModel : PageModel
    {
        private readonly ILogger<BranchPageModel> _logger;
        private readonly HttpClient _httpClient;
        private List<Branch> Branches;

        public BranchPageModel(ILogger<BranchPageModel> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public void OnGet()
        {
            var branches = _httpClient.GetAsync("https://localhost:44327/api/branch/all").GetAwaiter().GetResult();
            Branches = CreateResponse<List<Branch>>(branches.Content);
        }

        private T CreateResponse<T>(HttpContent response)
        {
            var stringData = response.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<T>(stringData);
        }
    }
}

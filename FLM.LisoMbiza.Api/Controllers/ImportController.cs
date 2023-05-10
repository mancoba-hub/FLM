using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        #region Properties

        private readonly ILogger<ImportController> _importLogger;
        private readonly IImportService _importService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportController"/> class.
        /// </summary>
        /// <param name="importLogger"></param>
        /// <param name="importService"></param>
        public ImportController(ILogger<ImportController> importLogger, IImportService importService)
        {
            _importLogger = importLogger;
            _importService = importService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Imports the branch file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fileBranch/")]
        public async Task ImportBranch(IFormFile file)
        {
            try
            {
                await _importService.ImportBranchList(file.OpenReadStream(), file.ContentType);                
            }
            catch (Exception e)
            {
                _importLogger.LogError($"An error occured", e);
                throw;
            }
        }

        /// <summary>
        /// Imports the product file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importProduct/")]
        public async Task ImportProduct(IFormFile file)
        {
            try
            {
                await _importService.ImportProductList(file.OpenReadStream(), file.ContentType);                
            }
            catch (Exception e)
            {
                _importLogger.LogError($"An error occured", e);
                throw;
            }
        }

        /// <summary>
        /// Imports the branch product file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("importBranchProduct/")]
        public async Task ImportBranchProduct(IFormFile file)
        {
            try
            {
                await _importService.ImportBranchProductList(file.OpenReadStream(), file.ContentType);
            }
            catch (Exception e)
            {
                _importLogger.LogError($"An error occured", e);
                throw;
            }
        }

        #endregion
    }
}

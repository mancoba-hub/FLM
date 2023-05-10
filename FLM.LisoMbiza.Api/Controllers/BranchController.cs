using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, MediaTypeNames.Text.Xml)]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, MediaTypeNames.Text.Xml)]
    public class BranchController : ControllerBase
    {
        #region Properties

        private readonly ILogger<BranchController> _branchLogger;
        private readonly IBranchService _branchService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchController"/> class.
        /// </summary>
        /// <param name="branchLogger"></param>
        /// <param name="branchService"></param>
        public BranchController(ILogger<BranchController> branchLogger, IBranchService branchService)
        {
            _branchLogger = branchLogger;
            _branchService = branchService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets all the product list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all/")]
        public async Task<Response<IEnumerable<Branch>>> GetBranchList()
        {
            try
            {
                var response = await _branchService.GetBranchListAsync();
                return ResponseMessage.ToResponseHelper(response);
            }
            catch (Exception exc)
            {
                _branchLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Gets the branch by id
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getById/")]
        public async Task<Response<Branch>> GetBranch(int branchId)
        {
            try
            {
                var branch = await _branchService.GetBranchAsync(branchId);
                return ResponseMessage.ToResponseHelper(branch);
            }
            catch (Exception exc)
            {
                _branchLogger.LogError($"An error occured", exc);
                throw;
            }            
        }

        /// <summary>
        /// Creates the branch
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create/")]
        public async Task CreateBranch([FromBody] Branch branch)
        {
            try
            {
                await _branchService.CreateBranchAsync(branch);
            }
            catch (Exception exc)
            {
                _branchLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Updates the branch
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/")]
        public async Task<Response<Branch>> UpdateBranch([FromBody] Branch branch)
        {
            try
            {
                var updatedBranch = await _branchService.UpdateBranchAsync(branch);
                return ResponseMessage.ToResponseHelper(updatedBranch);
            }
            catch (Exception exc)
            {
                _branchLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        /// <summary>
        /// Deletes the branch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("delete/")]
        public async Task<Response<bool>> DeleteBranch(int id)
        {
            try
            {
                var response = await _branchService.DeleteBranchAsync(id);
                return ResponseMessage.ToResponseHelper(response);
            }
            catch (FLMException exc)
            {
                return ResponseMessage.ToResponseHelper(false, exc.Message, null, false);
            }
            catch (Exception exc)
            {
                _branchLogger.LogError($"An error occured", exc);
                throw;
            }
        }

        #endregion
    }
}

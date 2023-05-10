using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public class BranchService : IBranchService
    {
        #region Properties

        private readonly IBranchRepository _branchRepository;
        private readonly IBranchProductRepository _branchProductRepository;

        #endregion

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchService"/> class.
        /// </summary>
        /// <param name="branchRepository"></param>
        /// <param name="branchProductRepository"></param>
        public BranchService(IBranchRepository branchRepository, IBranchProductRepository branchProductRepository)
        {
            _branchRepository = branchRepository;
            _branchProductRepository = branchProductRepository;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Creates the branch asynchronously
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public async Task CreateBranchAsync(Branch branch)
        {
            await _branchRepository.CreateBranchAsync(branch);
        }

        /// <summary>
        /// Creates the branch list asynchronously
        /// </summary>
        /// <param name="branchList"></param>
        /// <returns></returns>
        public async Task CreateBranchListAsync(List<Branch> branchList)
        {
            await _branchRepository.CreateBranchListAsync(branchList);
        }

        /// <summary>
        /// Deletes the branch asynchronously
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBranchAsync(int branchId)
        {
            var branchProductList = await _branchProductRepository.GetBranchProductListAsync();
            var branchProduct = branchProductList.Where(x => x.BranchID == branchId);
            if (branchProduct.Any())
                throw new FLMException("The Branch cannot be deleted as it is linked to a product");

            return await _branchRepository.DeleteBranchAsync(branchId);
        }

        /// <summary>
        /// Gets the branch asynchronously
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<Branch> GetBranchAsync(int branchId)
        {
            return await _branchRepository.GetBranchAsync(branchId);
        }

        /// <summary>
        /// Gets the branch list asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Branch>> GetBranchListAsync()
        {
            return await _branchRepository.GetAllBranchAsync();
        }

        /// <summary>
        /// Updates the branch asynchronously
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public async Task<Branch> UpdateBranchAsync(Branch branch)
        {
            return await _branchRepository.UpdateBranchAsync(branch);
        }

        #endregion
    }
}

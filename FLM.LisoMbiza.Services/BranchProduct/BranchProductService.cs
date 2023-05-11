using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    public class BranchProductService : IBranchProductService
    {
        #region Properties

        private readonly IBranchProductRepository _branchProductRepository;

        #endregion

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchProductService"/> class.
        /// </summary>
        /// <param name="branchRepository"></param>
        public BranchProductService(IBranchProductRepository branchProductRepository)
        {
            _branchProductRepository = branchProductRepository;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Create the branch products
        /// </summary>
        /// <param name="branchProducts"></param>
        /// <returns></returns>
        public async Task CreateBranchProduct(List<BranchProduct> branchProducts)
        {
            await _branchProductRepository.CreateBranchProductListAsync(branchProducts);
        }

        /// <summary>
        /// Gets the branch product
        /// </summary>
        /// <param name="branchProductId"></param>
        /// <returns></returns>
        public async Task GetBranchProduct(int branchProductId)
        {
            await _branchProductRepository.GetBranchProductAsync(branchProductId);
        }

        #endregion
    }
}

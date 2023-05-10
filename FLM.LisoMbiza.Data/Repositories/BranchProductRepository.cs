using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    public class BranchProductRepository : BaseRepository<BranchProduct>, IBranchProductRepository
    {
        #region Properties

        private readonly FLMContext _flmContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchProductRepository"/> class.
        /// </summary>
        /// <param name="flmContext"></param>
        public BranchProductRepository(FLMContext flmContext) : base(flmContext)
        {
            _flmContext = flmContext;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Create the branch product asynchronously
        /// </summary>
        /// <param name="branchProduct"></param>
        /// <returns></returns>
        public async Task CreateBranchProductAsync(BranchProduct branchProduct)
        {
            Create(branchProduct);
            _flmContext.SaveChanges();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Create the branch product list asynchronously
        /// </summary>
        /// <param name="branchProductList"></param>
        /// <returns></returns>
        public async Task CreateBranchProductListAsync(List<BranchProduct> branchProductList)
        {
            foreach (var branchProduct in branchProductList)
            {
                await CreateBranchProductAsync(branchProduct);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Gets the branch product asynchronously
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BranchProduct>> GetBranchProductAsync(int id)
        {
            return await GetAllByQueryAsync(x => x.ProductID == id || x.BranchID == id);
        }

        /// <summary>
        /// Gets the branch product list asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BranchProduct>> GetBranchProductListAsync()
        {
            return await GetAllAsync();
        }

        /// <summary>
        /// Deletes the product branch
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task DeleteProductBranchAsync(int productId)
        {
            var products = await GetBranchProductAsync(productId);
            foreach(var product in products)
            {
                await DeleteAsync(product);
            }
            _flmContext.SaveChanges();
            await Task.CompletedTask;
        }

        #endregion
    }
}

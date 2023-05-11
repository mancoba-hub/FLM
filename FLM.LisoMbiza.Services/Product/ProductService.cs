using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public class ProductService : IProductService
    {
        #region Properties

        private readonly IProductRepository _productRepository;
        private readonly IBranchProductRepository _branchProductRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="productRepository"></param>
        /// <param name="branchProductRepository"></param>
        public ProductService(IProductRepository productRepository, IBranchProductRepository branchProductRepository)
        {
            _productRepository = productRepository;
            _branchProductRepository = branchProductRepository;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Creates the product asynchronously
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task CreateProductAsync(Product product)
        {
            await _productRepository.CreateProductAsync(product);            
        }

        /// <summary>
        /// Creates the product list asynchronously
        /// </summary>
        /// <param name="productList"></param>
        /// <returns></returns>
        public async Task CreateProductListAsync(List<Product> productList)
        {
            await _productRepository.CreateProductListAsync(productList);
        }

        /// <summary>
        /// Deletes the product asynchronously
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProductAsync(int productId)
        {
            var branchProductList = await _branchProductRepository.GetBranchProductListAsync();
            var branchProduct = branchProductList.Where(x => x.ProductID == productId);
            if (branchProduct.Any())
                throw new FLMException("The Product cannot be deleted as it is linked to a branch");

            return await _productRepository.DeleteProductAsync(productId);
        }

        /// <summary>
        /// Gets the product asynchronously
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Product> GetProductAsync(int productId)
        {
            return await _productRepository.GetProductAsync(productId);
        }

        /// <summary>
        /// Gets the product list asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetProductListAsync()
        {
            return await _productRepository.GetAllProductAsync();
        }

        /// <summary>
        /// Updates the product asynchronously
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> UpdateProductAsync(Product product)
        {
            return await _productRepository.UpdateProductAsync(product);
        }

        /// <summary>
        /// Gets the branch products asynchronously
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BranchProduct>> GetBranchProductAsync(int productId)
        {
            return await _branchProductRepository.GetBranchProductAsync(productId);
        }

        /// <summary>
        /// Deletes the product bramch asynchronously
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProductBranch(int productId)
        {
            await _branchProductRepository.DeleteProductBranchAsync(productId);
            return true;
        }

        /// <summary>
        /// Creates the branch product list asynchronously
        /// </summary>
        /// <param name="branchProductList"></param>
        /// <returns></returns>
        public async Task CreateBranchProductListAsync(List<BranchProduct> branchProductList)
        {
            await _branchProductRepository.CreateBranchProductListAsync(branchProductList);
        }

        /// <summary>
        /// Gets the branch product list asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BranchProduct>> GetBranchProductListAsync()
        {
            return await _branchProductRepository.GetBranchProductListAsync();
        }

        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FLM.LisoMbiza
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        #region Properties

        private readonly FLMContext _flmContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="flmContext"></param>
        public ProductRepository(FLMContext flmContext): base(flmContext)
        {
            _flmContext = flmContext;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Creates the product asynchronously
        /// </summary>
        /// <param name="product"></param>
        public async Task CreateProductAsync(Product product)
        {
            if (product.ID == 0)
                product.ID = await GetMaxId();

            await CreateAsync(product);
            await _flmContext.SaveChangesAsync();
        }

        /// <summary>
        /// Creates the product list asynchronously
        /// </summary>
        /// <param name="productList"></param>
        /// <returns></returns>
        public async Task CreateProductListAsync(List<Product> productList)
        {
            foreach (var product in productList)
            {
                await CreateProductAsync(product);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Deletes the branch asynchronously
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProductAsync(int productId)
        {
            bool response = false;
            var product = await GetByQueryAsync(x => x.ID == productId);
            if (product != default)
                response = await DeleteAsync(product);

            await _flmContext.SaveChangesAsync();
            return response;
        }

        /// <summary>
        /// Gets all the products asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await GetAllAsync();
        }

        /// <summary>
        /// Gets the product asynchronously
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<Product> GetProductAsync(int productId)
        {
            return await GetByQueryAsync(x => x.ID == productId);
        }

        /// <summary>
        /// Updates the product asynchronously
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> UpdateProductAsync(Product product)
        {
            var response = await UpdateAsync(product);
            await _flmContext.SaveChangesAsync();
            return await GetByQueryAsync(x => x.ID == product.ID);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the max id
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetMaxId()
        {
            return await _flmContext.Product.MaxAsync(x => x.ID) + 1;
        }

        #endregion
    }
}

using System.Threading.Tasks;
using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public interface IProductRepository
    {
        Task CreateProductAsync(Product product);

        Task CreateProductListAsync(List<Product> productList);

        Task<Product> UpdateProductAsync(Product product);

        Task<Product> GetProductAsync(int productId);

        Task<IEnumerable<Product>> GetAllProductAsync();

        Task<bool> DeleteProductAsync(int productId);
    }
}

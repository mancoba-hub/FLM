using System.Threading.Tasks;
using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public interface IProductService
    {
        Task CreateProductAsync(Product product);

        Task CreateProductListAsync(List<Product> productList);

        Task<Product> GetProductAsync(int productId);

        Task<Product> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(int productId);

        Task<IEnumerable<Product>> GetProductListAsync();

        Task<IEnumerable<BranchProduct>> GetBranchProductAsync(int productId);

        Task<bool> DeleteProductBranch(int productId);

        Task CreateBranchProductListAsync(List<BranchProduct> branchProductList);

        Task<IEnumerable<BranchProduct>> GetBranchProductListAsync();
    }
}

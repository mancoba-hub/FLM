using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    public interface IBranchProductRepository
    {
        Task CreateBranchProductAsync(BranchProduct branchProduct);

        Task CreateBranchProductListAsync(List<BranchProduct> branchProductList);

        Task<IEnumerable<BranchProduct>> GetBranchProductAsync(int branchProductId);

        Task DeleteProductBranchAsync(int productId);

        Task<IEnumerable<BranchProduct>> GetBranchProductListAsync();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    public interface IBranchProductService
    {
        Task CreateBranchProduct(List<BranchProduct> branchProducts);

        Task GetBranchProduct(int branchProductId);
    }
}

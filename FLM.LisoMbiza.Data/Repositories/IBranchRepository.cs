using System.Threading.Tasks;
using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public interface IBranchRepository
    {
        Task CreateBranchAsync(Branch branch);

        Task CreateBranchListAsync(List<Branch> branchList);

        Task<Branch> UpdateBranchAsync(Branch branch);

        Task<Branch> GetBranchAsync(int branchId);

        Task<IEnumerable<Branch>> GetAllBranchAsync();

        Task<bool> DeleteBranchAsync(int branchId);
    }
}

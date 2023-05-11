using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    public interface IBranchService
    {
        Task CreateBranchAsync(Branch branch);

        Task CreateBranchListAsync(List<Branch> branchList);

        Task<Branch> GetBranchAsync(int branchId);

        Task<Branch> UpdateBranchAsync(Branch branch);

        Task<bool> DeleteBranchAsync(int branchId);

        Task<IEnumerable<Branch>> GetBranchListAsync();
    }
}

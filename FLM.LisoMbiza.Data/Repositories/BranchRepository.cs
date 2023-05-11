using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FLM.LisoMbiza
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        #region Properties

        private readonly FLMContext _flmContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BranchRepository"/> class.
        /// </summary>
        /// <param name="flmContext"></param>
        public BranchRepository(FLMContext flmContext) : base(flmContext)
        {
            _flmContext = flmContext;
        }

        #endregion

        #region Implemented Members

        /// <summary>
        /// Created the branch asynchronously
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public async Task CreateBranchAsync(Branch branch)
        {
            if (branch.ID == 0)
                branch.ID = await GetMaxId();

            await CreateAsync(branch);
            await _flmContext.SaveChangesAsync();
        }

        /// <summary>
        /// Creates the branch list asynchronously
        /// </summary>
        /// <param name="branchList"></param>
        /// <returns></returns>
        public async Task CreateBranchListAsync(List<Branch> branchList)
        {
            foreach(var branch in branchList)
            {
                if (branch.OpenDate == System.DateTime.MinValue)
                    branch.OpenDate = null;

                await CreateBranchAsync(branch);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Updates the branch asynchronously
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public async Task<Branch> UpdateBranchAsync(Branch branch)
        {
            await UpdateAsync(branch);
            await _flmContext.SaveChangesAsync();
            return await GetBranchAsync(branch.ID);
        }

        /// <summary>
        /// Gets the branch asynchronously
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<Branch> GetBranchAsync(int branchId)
        {
            return await GetByQueryAsync(x => x.ID == branchId);
        }

        /// <summary>
        /// Gets all branches asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Branch>> GetAllBranchAsync()
        {
            return await GetAllAsync();
        }

        /// <summary>
        /// Deletes the branch asynchronously
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBranchAsync(int branchId)
        {
            bool response = false;
            var branch = await GetByQueryAsync(x => x.ID == branchId);
            if (branch != default)
                response = await DeleteAsync(branch);

            _flmContext.BranchProduct.FromSqlRaw<BranchProduct>($"SELECT COUNT(1) FROM [dbo].[BranchProduct] WHERE BranchId = {branchId}");
            _flmContext.SaveChanges();
            return response;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the max id
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetMaxId()
        {
            return await _flmContext.Branch.MaxAsync(x => x.ID) + 1;
        } 

        #endregion
    }
}

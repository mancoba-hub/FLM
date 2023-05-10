using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public class ReportBranch
    {
        public int BranchId { get; set; }

        public string BranchName { get; set; }

        public List<ReportProduct> BranchProducts { get; set; }
    }
}

using System.Collections.Generic;

namespace FLM.LisoMbiza
{
    public class ReportProduct
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public List<ReportBranch> ProductBranches { get; set; }
    }
}

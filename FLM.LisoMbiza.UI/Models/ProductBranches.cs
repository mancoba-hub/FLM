using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLM.LisoMbiza
{
    public class ProductBranches
    {
        public int ID { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public bool WeightedItem { get; set; }

        public decimal SuggestedSellingPrice { get; set; }

        public List<BranchProducts> Branches { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class BranchProducts
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}

using System;

namespace FLM.LisoMbiza
{
    [Serializable]
    public class Product : BaseEntity
    {
        public bool WeightedItem { get; set; } 

        public decimal SuggestedSellingPrice { get; set; }
    }
}

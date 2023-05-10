using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLM.LisoMbiza
{
    [Serializable]
    public class BranchProduct
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("BranchId")]
        public int BranchID { get; set; }

        [ForeignKey("ProductId")]
        public int ProductID { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FLM.LisoMbiza
{
    [Serializable]
    public class Branch : BaseEntity
    {
        [MaxLength(15)]
        public string TelephoneNumber { get; set; }

        public DateTime? OpenDate { get; set; }
    }
}

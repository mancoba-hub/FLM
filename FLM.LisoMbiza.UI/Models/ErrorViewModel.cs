using System;

namespace FLM.LisoMbiza.UI.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string SourceMessage { get; set; }

        public string ErrorMessage { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

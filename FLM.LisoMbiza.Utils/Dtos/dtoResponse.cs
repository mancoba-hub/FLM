using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLM.LisoMbiza
{
    public class Response
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public bool Data { get; set; }
    }
}

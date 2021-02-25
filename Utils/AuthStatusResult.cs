using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Utils
{
    public class AuthStatusResult
    {
        public bool isSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public AuthStatusResult()
        {
            isSuccessful = true;
            ErrorMessage = "";
        }
    }
}

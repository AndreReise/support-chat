using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace support_chat.Utils
=======
namespace TechnicalSupport.Utils
>>>>>>> 278f6d21b3af26581a8e15dbbb5b837aaffb30ea
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

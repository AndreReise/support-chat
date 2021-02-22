using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Services
{
    public interface ICryptoProvider
    {
        byte[] GetPasswordHash(string str_password, byte[] l_salt);
        byte[] GetRandomSaltString(int str_length);
        byte[] GetSHA256Hash(byte[] target);
    }
}

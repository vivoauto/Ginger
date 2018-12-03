using System;
using System.Collections.Generic;
using System.Text;

namespace Amdocs.Ginger.Common
{
    public interface IEncryptionHandler
    {
        string EncryptString(string strToEncrypt, ref bool result);

        string DecryptString(string strToDecrypt, ref bool result);
    }
}

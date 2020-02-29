using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Data
{
    public interface ICurrencyCodeManager
    {
        bool CheckCode(string code);
    }
}

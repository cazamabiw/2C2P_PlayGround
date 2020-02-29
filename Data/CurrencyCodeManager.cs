using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Data
{
    public class CurrencyCodeManager : ICurrencyCodeManager
    {
        MyDbContext _context;

        public CurrencyCodeManager(MyDbContext context)
        {
            this._context = context;
        }
        public bool CheckCode(string code)
        {
            return _context.CurrencyCode.Any(f => f.Code == code);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Models.Helpers
{
    public class ValidateResult
    {
        public bool Success { get; set; }
        public string Msg { get; set; }

        public Transaction tran { get; set; }
    }
}

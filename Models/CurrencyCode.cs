using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Models
{
    [Table("CurrencyCodes")]
    public class CurrencyCode
    {
        [Key]
        public string Code { get; set; }
    }
}

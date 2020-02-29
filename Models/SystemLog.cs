using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Models
{
    [Table("SystemLogs")]
    public class SystemLog
    {
        [Key]
        public string Id { get; set; }

        public string Catagory { get; set; }
        public bool Success { get; set; }
        public string Description { get; set; }

        public string ErrorMsg { get; set; }
        public DateTime LogDateUTC { get; set; }
    }
}

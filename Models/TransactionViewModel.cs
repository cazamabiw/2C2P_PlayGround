using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Models
{
    public class TransactionViewModel
    {
        public List<TransactionResponse> Transactions { get; set; }
        public SelectList Category { get; set; } 
        public string CategoryItem { get; set; } 
        public string SearchString { get; set; }

        public SelectList Currency { get; set; } 
        public string CodeItem { get; set; } 

        public SelectList Status { get; set; } 
        public string StatusItem { get; set; } 


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public TransactionViewModel()
        {
            StartDate = DateTime.Now.AddDays(-1);
            EndDate = DateTime.Now;
        }
    }
}

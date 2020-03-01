using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2C2PTest.Data;
using _2C2PTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2C2PTest.Controllers
{
    public class TransactionController : Controller
    {
        ITransactionManager _tm;
        MyDbContext _context;
        public TransactionController(ITransactionManager tm,MyDbContext context)
        {  
            _tm = tm;
            _context = context;
        }
        public ActionResult Index(string categoryItem, DateTime startDate, DateTime endDate, string codeItem, string statusItem)
        {

            var trans = GetAll();

            if (categoryItem == "Currency")
            {
                trans = GetByCurrency(codeItem);
            }
            else if (categoryItem == "Date range")
            {

                trans = GetByDateRange(startDate, endDate);
            }
            else if (categoryItem == "Status")
            {
                trans = GetByStatus(statusItem);
            }

            var tranVM = new TransactionViewModel
            {
                Category = new SelectList(new List<string>() { "Currency", "Date range", "Status" }),
                Transactions = trans,
                Status = new SelectList(new List<string>() { "A", "R", "D" }),
                Currency = new SelectList(_context.CurrencyCode.Select(f => f.Code))
            };
            return View(tranVM);
        }



        [HttpGet]
        [Route("api/Transaction/GetAll")]
        public List<TransactionResponse> GetAll()
        {
            return _tm.GetAll();
        }

        [HttpGet]
        [Route("api/Transaction/GetByCurrency")]
        public List<TransactionResponse> GetByCurrency(string query)
        {
            return _tm.GetByCurrency(query);
        }

        [HttpGet]
        [Route("api/Transaction/GetByDateRange")]
        public List<TransactionResponse> GetByDateRange(DateTime start, DateTime end)
        {
            return _tm.GetByDateRange(start,end);
        }

        [HttpGet]
        [Route("api/Transaction/GetByStatus")]
        public List<TransactionResponse> GetByStatus(string query)
        {
            return _tm.GetBystatus(query);
        }
    }
}
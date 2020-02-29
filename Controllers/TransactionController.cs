using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2C2PTest.Data;
using _2C2PTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2C2PTest.Controllers
{
    public class TransactionController : Controller
    {
        ITransactionManager _tm;
        public TransactionController(ITransactionManager tm)
        {
       
            _tm = tm;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Transaction/GetAll")]
        public List<TransactionResponse> GetAll()
        {
            return _tm.GetAll();
        }
    }
}
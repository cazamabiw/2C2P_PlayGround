using _2C2PTest.Models;
using _2C2PTest.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Data
{
    public class TransactionManager : ITransactionManager
    {
        MyDbContext _context;

        public TransactionManager(MyDbContext context)
        {
            this._context = context;
        }

        public void CreateLog(string msg, string category, string description)
        {
            throw new NotImplementedException();
        }

        public List<TransactionResponse> GetAll()
        {
            var data = _context.Transaction.ToList();
            var tranResponse = new List<TransactionResponse>();
            tranResponse = data.ConvertAll(f => new TransactionResponse
            {
                id = f.TransactionId,
                payment = String.Format("{0} {1}" ,f.Amount.ToString(),f.CurrencyCode),
                Status = f.Status

            });

            return tranResponse;

        }

        public List<TransactionResponse> GetByCurrency(string query)
        {
            throw new NotImplementedException();
        }

        public List<TransactionResponse> GetByDateRange(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public List<TransactionResponse> GetBystatus(string query)
        {
            throw new NotImplementedException();
        }

        public Result SaveTransactions(List<Transaction> transactions)
        {
            throw new NotImplementedException();
        }
    }
}
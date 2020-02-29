using _2C2PTest.Models;
using _2C2PTest.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Data
{
    public interface ITransactionManager
    {
        List<TransactionResponse> GetAll();
        List<TransactionResponse> GetByCurrency(string query);
        List<TransactionResponse> GetByDateRange(DateTime start, DateTime end);
        List<TransactionResponse> GetBystatus(string query);
        Result SaveTransactions(List<Transaction> transactions);

        void CreateLog(string msg, string category, string description);
    }
}

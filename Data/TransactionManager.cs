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
            var log = new SystemLog();

            log.Id = Guid.NewGuid().ToString();
            log.LogDateUTC = DateTime.UtcNow;
            log.Description = description;
            log.ErrorMsg = msg;
            log.Catagory = category;
            _context.SystemLog.Add(log);
            _context.SaveChanges();
        }

        public List<TransactionResponse> GetAll()
        {
            var data = _context.Transaction.OrderBy(f => f.TransactionDate).ToList();
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
            var data = _context.Transaction.Where(f=>f.CurrencyCode == query).OrderBy(f=>f.TransactionDate).ToList();
            var tranResponse = new List<TransactionResponse>();
            tranResponse = data.ConvertAll(f => new TransactionResponse
            {
                id = f.TransactionId,
                payment = String.Format("{0} {1}", f.Amount.ToString(), f.CurrencyCode),
                Status = f.Status

            });

            return tranResponse;
        }

        public List<TransactionResponse> GetByDateRange(DateTime start, DateTime end)
        {
            var data = _context.Transaction.Where(f => f.TransactionDate >= start && f.TransactionDate <= end).OrderBy(f => f.TransactionDate).ToList();
            var tranResponse = new List<TransactionResponse>();
            tranResponse = data.ConvertAll(f => new TransactionResponse
            {
                id = f.TransactionId,
                payment = String.Format("{0} {1}", f.Amount.ToString(), f.CurrencyCode),
                Status = f.Status

            });

            return tranResponse;
        }

        public List<TransactionResponse> GetBystatus(string query)
        {
            var data = _context.Transaction.Where(f => f.Status == query).OrderBy(f => f.TransactionDate).ToList();
            var tranResponse = new List<TransactionResponse>();
            tranResponse = data.ConvertAll(f => new TransactionResponse
            {
                id = f.TransactionId,
                payment = String.Format("{0} {1}", f.Amount.ToString(), f.CurrencyCode),
                Status = f.Status

            });

            return tranResponse;
        }

        public Result SaveTransactions(List<Transaction> transactions)
        {
            Result res = new Result();
            try
            {
                foreach (var tran in transactions)
                {
                    if (!_context.Transaction.Any(f => f.TransactionId == tran.TransactionId))
                    {
                        _context.Transaction.Add(tran);
                        _context.SaveChanges();

                    }

                }
                res.Success = true;
                res.Msg = "Sucess";
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Msg = ex.Message;
                return res;
            }
        }
    }
}
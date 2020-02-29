using _2C2PTest.Models;
using _2C2PTest.Models.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace _2C2PTest.Data
{
    public class FileManager : IFileManager
    {
        ITransactionManager _tm;
        ICurrencyCodeManager _cm;
        public FileManager(ITransactionManager tm, ICurrencyCodeManager cm)
        {
            _tm = tm;
            _cm = cm;
        }

        public Result ConvertFile(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            var result = new Result();
            if (fileExtension == ".csv")
            {
                return ConvertCSV(file);
            }
            else
            {

                return ConvertXML(file);

            }
        }

        #region "CSV"
        public Result ConvertCSV(IFormFile file)
        {
            var res = new Result();
            int record = 0;

            try
            {
                List<Transaction> tranList = new List<Transaction>();
                using (var sreader = new StreamReader(file.OpenReadStream()))
                {
                    var eee = sreader;

                    while (!sreader.EndOfStream)
                    {
                        record++;
                        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        string[] Fields = CSVParser.Split(sreader.ReadLine());

                        for (int i = 0; i < Fields.Length; i++)
                        {
                            Fields[i] = Fields[i].TrimStart(' ', '"');
                            Fields[i] = Fields[i].TrimEnd('"');
                        }

                        var v = ValidateCSVFormat(Fields, file.FileName);

                        if (v.Success == true)
                        {
                            tranList.Add(v.tran);
                        }
                    }

                }
                if (tranList.Count() != record || record == 0)
                {
                    res.Success = false;
                    res.Msg = "Record is invalid format";
                    return res;
                }
                res = _tm.SaveTransactions(tranList);
                return res;


            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Msg = ex.Message;
                return res;
            }
        }

        public ValidateResult ValidateCSVFormat(string[] fields, string fileName)
        {
            ValidateResult res = new ValidateResult();
            res.Success = true;
            res.Msg = "";

            if (fields.Length < 5)
            {
                res.Success = false;
                res.Msg = "Record is missing data";
                _tm.CreateLog(res.Msg, "Record Invalid Format", string.Format("{0} Record {1}", fileName, string.Join(",", fields)));

                return res;
            }


            List<string> status = new List<string>() { "Approved", "Failed", "Finished" };

            #region "check format Transaction identificator"
            if (fields[0].Length > 50)
            {
                res.Success = false;
                res.Msg = "Transaction identificator is max length 50. ";
            }
            #endregion

            #region"Check Amount decimal format"
            decimal d;
            if (!decimal.TryParse(fields[1], out d))
            {
                res.Success = false;
                res.Msg = res.Msg + "Amount must be decimal.";
            }
            #endregion

            #region "check format Currency ISO4217 format"
            if (_cm.CheckCode(fields[2]) == false)
            {
                res.Success = false;
                res.Msg = res.Msg + "Currency code must be ISO4217.";
            }
            #endregion

            #region "check format TransactionDate"
            DateTime dt;

            //hh-> 12-hour clock.so use HH for support 24-hour.
            var valid = DateTime.TryParseExact(fields[3], "dd'/'MM'/'yyyy HH:mm:ss", null, DateTimeStyles.None, out dt);
            if (!valid)
            {
                res.Success = false;
                res.Msg = res.Msg + "Transaction Date must be format dd/MM/yyyy hh:mm:ss.";
            }
            #endregion

            #region "check status"
            if (!status.Contains(fields[4]))
            {
                res.Success = false;
                res.Msg = res.Msg + "Invalid Status.";
            }
            #endregion


            //collect to log
            if (res.Success == false)
            {
                _tm.CreateLog(res.Msg, "Record Invalid Format", string.Format("{0} Record {1}", fileName, string.Join(",", fields)));

            }
            else
            {
                res.tran = new Transaction();
                res.tran.TransactionId = fields[0].ToString();
                res.tran.Amount = decimal.Parse(fields[1].ToString());
                res.tran.CurrencyCode = fields[2].ToString();
                res.tran.TransactionDate = DateTime.Parse(fields[3].ToString());
                res.tran.Status = MapStatus(fields[4]).ToString();
            }
            return res;
        }
        #endregion

        #region "XML"
        public Result ConvertXML(IFormFile file)
        {
            var res = new Result();
            try
            {
                List<Transaction> tranList = new List<Transaction>();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file.OpenReadStream());
                XmlNodeList tranNodes = xmlDocument.SelectNodes("Transactions/Transaction");
                foreach (XmlNode node in tranNodes)
                {

                    var v = ValidateXMLFormat(node, file.FileName);
                    if (v.Success == true)
                    {
                        tranList.Add(v.tran);
                    }
                }

                if (tranList.Count() != tranNodes.Count || tranNodes.Count == 0)
                {
                    res.Success = false;
                    res.Msg = "Record is invalid format";
                    return res;
                }

                res = _tm.SaveTransactions(tranList);
                return res;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Msg = ex.Message;
                return res;
            }
        }

        public ValidateResult ValidateXMLFormat(XmlNode node, string fileName)
        {
            ValidateResult res = new ValidateResult();
            res.Success = true;
            res.Msg = "";


            try
            {
                var transactionId = node.Attributes["id"].Value;
                var transactionDate = node["TransactionDate"].InnerText;
                var status = node["Status"].InnerText;
                var amount = node.SelectSingleNode("PaymentDetails")["Amount"].InnerText;
                var currencyCode = node.SelectSingleNode("PaymentDetails")["CurrencyCode"].InnerText;


                #region "check format Transaction identificator"
                if (transactionId.Length > 50)
                {
                    res.Success = false;
                    res.Msg = "Transaction identificator is max length 50.";
                }
                #endregion

                #region"Check Amount decimal format"
                decimal d;
                if (!decimal.TryParse(amount, out d))
                {
                    res.Success = false;
                    res.Msg = res.Msg + "Amount must be decimal.";
                }
                #endregion

                #region "check format Currency ISO4217 format"
                if (_cm.CheckCode(currencyCode) == false)
                {
                    res.Success = false;
                    res.Msg = res.Msg + "Currency code must be ISO4217.";
                }
                #endregion

                #region "check format TransactionDate"
                DateTime dt;

                //hh-> 12-hour clock.so use HH for support 24-hour.
                var valid = DateTime.TryParseExact(transactionDate, "yyyy-MM-dd\'T\'HH:mm:ss", null, DateTimeStyles.None, out dt);
                if (!valid)
                {
                    res.Success = false;
                    res.Msg = res.Msg + "Transaction Date must be format yyyy-MM-ddThh:mm:ss.";
                }
                #endregion

                #region "check status"
                if (!status.Contains(status))
                {
                    res.Success = false;
                    res.Msg = res.Msg + "Invalid Status.";
                }
                #endregion

                //collect to log
                if (res.Success == false)
                {
                    _tm.CreateLog(res.Msg, "Record Invalid Format", string.Format("{0} Data {1}", fileName, node.InnerXml));

                }
                else
                {
                    res.tran = new Transaction();
                    res.tran.TransactionId = transactionId;
                    res.tran.Amount = decimal.Parse(amount);
                    res.tran.CurrencyCode = currencyCode;
                    res.tran.TransactionDate = DateTime.Parse(transactionDate);
                    res.tran.Status = MapStatus(status).ToString();
                }
                return res;
            }
            catch
            {
                res.Success = false;
                res.Msg = "Record is missing data";
                _tm.CreateLog(res.Msg, "Record Invalid Format", string.Format("{0} Data {1}", fileName, node.InnerXml));

                return res;
            }
        }

        #endregion
        public StatusLevel MapStatus(string status)
        {
            var dict = new Dictionary<string, StatusLevel> {
            { "Approved", StatusLevel.A },
            { "Failed", StatusLevel.R },
            { "Rejected", StatusLevel.R },
            { "Finished", StatusLevel.D },
            { "Done", StatusLevel.D }
            };
            return dict[status];
        }

      

 
    }
}

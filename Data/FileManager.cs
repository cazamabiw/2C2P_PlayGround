using _2C2PTest.Models.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Data
{
    public class FileManager : IFileManager
    {
        public Result ConvertCSV(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Result ConvertFile(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Result ConvertXML(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public StatusLevel MapStatus(string status)
        {
            throw new NotImplementedException();
        }
    }
}

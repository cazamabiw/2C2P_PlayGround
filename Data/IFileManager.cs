using _2C2PTest.Models.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2C2PTest.Data
{
    public interface IFileManager
    {
        Result ConvertFile(IFormFile file);
        Result ConvertCSV(IFormFile file);
        Result ConvertXML(IFormFile file);
        StatusLevel MapStatus(string status);
    }
}

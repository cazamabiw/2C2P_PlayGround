using _2C2PTest.Models.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace _2C2PTest.Data
{
    public interface IFileManager
    {
        Result ConvertFile(IFormFile file);
        Result ConvertCSV(IFormFile file);
        Result ConvertXML(IFormFile file);
        ValidateResult ValidateCSVFormat(string[] fields, string fileName);
        ValidateResult ValidateXMLFormat(XmlNode node, string fileName);
        StatusLevel MapStatus(string status);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _2C2PTest.Data;
using _2C2PTest.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _2C2PTest.Controllers
{
    public class UploadFileController : Controller
    {
        IFileManager _fm;

        public UploadFileController(IFileManager fm)
        {
            _fm = fm;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UploadFile")]
        public ActionResult Index(IFormFile file)
        {
            Result result;
            try
            {
                if(file == null)
                {
                    ViewBag.validatetxt = "Please choose your file";
                    return View();
                }
                string fileExtension = Path.GetExtension(file.FileName);

                if (file.Length > 1000000)
                {
                    ViewBag.validatetxt = "File size is max more than 1 MB !!!!";
                    return View();

                }

                if (fileExtension != ".csv" && fileExtension != ".xml")
                {
                    return BadRequest("Unknown format");
                }
                else
                {
                    result = _fm.ConvertFile(file);
                    if (result.Success == true)
                    {
                        return Ok(StatusCode(200).StatusCode);
                    }
                    else
                    {
                        return BadRequest(result.Msg);
                    }
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
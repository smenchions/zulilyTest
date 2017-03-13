using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zulilySurvey.Data;
using zulilySurvey.Entities;
using MongoDB.Bson;

namespace zulilySurvey.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var temp = HttpContext.Request.Cookies["User"];
            return View();
        }
    }
}

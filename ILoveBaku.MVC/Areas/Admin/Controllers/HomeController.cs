using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILoveBaku.MVC.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILoveBaku.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
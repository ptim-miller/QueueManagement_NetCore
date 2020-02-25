using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QMS.Models;

namespace QMS.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return RedirectToAction("Start","Patients");
        }

        public IActionResult Error(bool newPatient = true)
        {
            TempData.Remove("patient");
            return View();
        }

    }
}
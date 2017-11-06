using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.Services;
using SaitynoProjektasBackEnd.Services.Interfaces;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("[controller]")]
    public class SilentController : Controller
    {
        IHostingEnvironment _environment;

        public SilentController(IHostingEnvironment environment) {
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index() {
            ViewData["FrontEndUrl"] = _environment.IsDevelopment() ? "http://localhost:3000" : "https://audiocloud.surge.sh";

            return View();
        }
    }
}

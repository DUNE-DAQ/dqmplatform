using DuneDaqMonitoringPlatform.Actions;
using DuneDaqMonitoringPlatform.Areas.Identity.Pages.Account;
using DuneDaqMonitoringPlatform.Data;
using DuneDaqMonitoringPlatform.Hubs;
using DuneDaqMonitoringPlatform.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Controllers
{
    
    public class HomeController : Controller
    {

        private readonly IHubContext<ChartHub> _hubContext;

        public static System.Timers.Timer myTimer = new System.Timers.Timer();




        public HomeController(IHubContext<ChartHub> hubContext)
        {
            _hubContext = hubContext;

        }


        public IActionResult Index()
        {
   
            return View();
        }      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

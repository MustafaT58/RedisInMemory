using Microsoft.AspNetCore.Mvc;
using RedisExchanceAPI.Web.Models;
using RedisExchangeApi.Web.Services;
using System.Diagnostics;

namespace RedisExchangeApi.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        RedisService _redisService;
        public HomeController(ILogger<HomeController> logger, RedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }

        public IActionResult Index()
        {
            _redisService.Connect();
            return View();
        }

        public IActionResult Privacy()
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
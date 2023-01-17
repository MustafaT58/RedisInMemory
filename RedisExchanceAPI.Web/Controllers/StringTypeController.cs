using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeApi.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase _db;
        public StringTypeController(RedisService redis)
        {
            _redis = redis;
            _db = redis.GetDB(0);
        }
        public IActionResult Index()
        {
            //var db = _redis.GetDB(0);
            //var db = _redisService.GetDB(0);
            _db.StringSet("name", "Mustafa Taşkıran");
            _db.StringSet("ziyaretci",100);

            return View();
        }
        public IActionResult Show()
        {
            //var value = _db.StringGet("name");

            var value = _db.StringGetRange("name",0, 3);
            
            //_db.StringIncrement("ziyaretci", 10);
            var count = _db.StringDecrementAsync("ziyaretci", 1).Result;
            _db.StringDecrementAsync("ziyaretci", 10).Wait();
            //if (value.HasValue)
            {
                ViewBag.value = value.ToString();
                ViewBag.count = count.ToString();
            }
            


            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchanceAPI.Web.Controllers
{
    public class SortedSetController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase _db;

        private string listKey = "hashnames";
        public SortedSetController(RedisService redis)
        {
            _redis = redis;
            _db = redis.GetDB(2);
        }
        public IActionResult Index()
        {
            HashSet<string> list = new HashSet<string>();
            if (_db.KeyExists(listKey))
            {
                _db.SortedSetScan(listKey).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });
                
            }


            return View(list);
        }
        [HttpPost]
        public  IActionResult Add(string name,int score)
        {
            _db.KeyExpire(listKey, DateTime.Now.AddMinutes(1));
            _db.SortedSetAdd(listKey, name, score);
            return RedirectToAction("Index");
        }
         public async Task<IActionResult> DeleteItem(string name)
        {
            _db.SetRemoveAsync(listKey, name); 


            return RedirectToAction("Index");
        }
    }
}

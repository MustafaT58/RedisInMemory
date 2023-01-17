using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchanceAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase _db;

        private string listKey="names";
        public ListTypeController(RedisService redis)
        {
            _redis = redis;
            _db = redis.GetDB(1);
        }
        public IActionResult Index()
        {
            List<string> nameList = new List<string>();
            if (_db.KeyExists(listKey))
            {
                _db.ListRange(listKey).ToList().ForEach(x =>
                {
                    nameList.Add(x.ToString());
                });
            }
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            _db.ListRightPush(listKey,name);
            return RedirectToAction("Index");
        }


        public IActionResult DeleteItem(string name)
        {
            _db.ListRemoveAsync(listKey,name).Wait();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteFirstItem()
        {
            _db.ListLeftPop(listKey);

            return RedirectToAction("Index");
        }

    }
}

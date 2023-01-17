using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchanceAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redis;
        private readonly IDatabase _db;

        private string listKey = "hashnames";
        public SetTypeController(RedisService redis)
        {
            _redis = redis;
            _db = redis.GetDB(2);
        }
        public IActionResult Index()
        {
            HashSet<string> namesList = new HashSet<string>();
            if (_db.KeyExists(listKey))
            {
                _db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
                  
            }
            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
           
            _db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            _db.SetAdd(listKey, name);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteItem(string name)
        {
            _db.SetRemoveAsync(listKey, name);


            return RedirectToAction("Index");
        }
    }
}

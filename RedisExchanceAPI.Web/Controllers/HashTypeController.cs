using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;
using System.Xml.Linq;

namespace RedisExchanceAPI.Web.Controllers
{
    public class HashTypeController : BaseController
    {
        public string hashKey { get; set; } = "sozluk";
        public HashTypeController(RedisService redis) : base(redis)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            if (_db.KeyExists(hashKey))
            {
                _db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    list.Add(x.Name, x.Value);
                });
            }

            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name,string val)
        {
            _db.HashSet(hashKey,name,val);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteItem(string name)
        {
            _db.HashDelete(hashKey, name);


            return RedirectToAction("Index");
        }
    }
}

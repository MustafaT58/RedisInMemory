using Microsoft.AspNetCore.Mvc;
using RedisExchangeApi.Web.Services;
using StackExchange.Redis;

namespace RedisExchanceAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redis;
        protected readonly IDatabase _db;

        public BaseController(RedisService redis)
        {
            _redis = redis;
            _db = redis.GetDB(1);
        }
    }
}

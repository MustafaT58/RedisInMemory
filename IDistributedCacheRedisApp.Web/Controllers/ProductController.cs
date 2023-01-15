using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache; 
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions=new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("names", "MUSTAFA",cacheEntryOptions);

            //await _distributedCache.SetStringAsync("surname", "Taskiran");

            Product product=new Product { Id = 1,Name="Kalem",Price=50 };
            string jsonproduct=JsonConvert.SerializeObject(product);
            //await _distributedCache.SetStringAsync("product:1", jsonproduct, cacheEntryOptions);

            Byte[] byteproduct = Encoding.UTF8.GetBytes(jsonproduct);
            _distributedCache.Set("product:1", byteproduct);


            return View();
        }

        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");
            string jsonproduct = Encoding.UTF8.GetString(byteProduct);
            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);
            ViewBag.product = p;


            //string name = _distributedCache.GetString("names");

            //string jsonproduct = _distributedCache.GetString("product:1");
            //Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);

            //ViewBag.product = p;




            return View();
        }
        public IActionResult Remove()
        {
           _distributedCache.Remove("names");


            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] resimbyte = _distributedCache.Get("resim");
            return File(resimbyte,"image/jpg");
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images/kartal.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim", imageByte);

            return View();
        }

    }
}

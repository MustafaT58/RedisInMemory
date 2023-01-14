using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {

            //1.yol
            //if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))   //zaman keyinin içi boş ise  Yenisini oluşturur
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}
            //2.yol



            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            //options.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10);   // AbsoluteExpiration 10 saniye sonra veriyi bellekten siler

            //AbsoluteExpiration ve SlidingExpiration aynı anda kullanıldığı zaman 10 saniye veriye erişilmezse kaybolur ve bir dakika sonra kesin bellekten silinir. 
            options.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(1);

            options.SlidingExpiration = TimeSpan.FromSeconds(10);                    //SlidingExpiration 10 saniye veriye erişilmezse bellekten siler erişilirse bellekte tutar.

            // options.Priority=CacheItemPriority.NeverRemove // Verilerin önemini belirlemek için kullanılır.Silme sırası belirlenir.

            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {   //RegisterPostEvictionCallback Silinen verinin neden silindiğini gösterir.

                _memoryCache.Set("callback", $"{key}->{value}=>sebep:{reason}");

            });


            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);


            Product p=new Product { Id = 1, Name="Kaem",Price=100 };
            _memoryCache.Set<Product>("product:1", p);


            return View();
        }
        public IActionResult Show()
        {
            //------------------DERS 1------------------------------------
            //_memoryCache.Remove("zaman");                                   //bu keye sahip olunan datayı memoryden siler.
            //_memoryCache.GetOrCreate<string>("zaman", entry =>              //Zaman var ise alır yoksa oluşturur. entry ile extra özellik eklenebilir.
            //{
            //    return DateTime.Now.ToString();
            //});
            //------------------DERS 1------------------------------------

            _memoryCache.TryGetValue("zaman", out string zamancache);
            ViewBag.zaman = zamancache;

            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.callback = callback;

            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Caching;
using Newtonsoft.Json;
using WebShop.Api.Interfaces;

namespace WebShop.Api.Implements
{
    public class CurrencyApi : ICurrencyConverter
    {
        private Uri _uri;
        private IHttpCache _cache;
        public CurrencyApi(Uri builder, IHttpCache cache)
        {
            _cache = cache;
            _uri = builder;
        }

        public virtual decimal ConvertUsdTo(decimal value, string convertTo)
        {
            var itemUsd = GetCoursies("usd");

            if (string.Equals(convertTo, "uah", StringComparison.OrdinalIgnoreCase))
            {
                return itemUsd.Sale * value;

            }
            if (string.Equals(convertTo, "usd", StringComparison.OrdinalIgnoreCase))
            {
                return value;
            }

            return value;
        }
        public decimal ConvertWithCeiling(decimal value, string convertTo)
        {
            return Math.Ceiling(ConvertUsdTo(value, convertTo));
        }
        private Currency GetCoursies(string nameCourse)
        {
            IEnumerable<Currency> result = _cache.TryGetValue("currency",() => InitializeActualCurrency(),
                new HttpCacheConfig()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Dependency = null,
                SlidingExpiration = Cache.NoSlidingExpiration,
                Callback = null,
                Priority = CacheItemPriority.Normal
            });

            var itemUsd = result.FirstOrDefault(c => c.Ccy.Equals(nameCourse, StringComparison.OrdinalIgnoreCase));
            return itemUsd;
        }

        private IEnumerable<Currency> InitializeActualCurrency()
        {
            HttpWebRequest request = WebRequest.CreateHttp(_uri);
            var result = request.GetResponse();
            IEnumerable<Currency> array;
            using (var stream = new StreamReader(result.GetResponseStream()))
            {
                array = JsonConvert.DeserializeObject<IEnumerable<Currency>>(stream.ReadToEnd());
            }
            return array;
        }
    }
}
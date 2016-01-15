using System;

namespace WebShop.Api.Storage
{
    public interface IHttpCacheConfig
    {

    }

    public interface IHttpCache
    {
       TResult TryGetValue<TResult>(string key, Func<TResult> value, IHttpCacheConfig config = null);
    }
}
using System;

namespace WebShop.Log.Abstract
{
    public interface ILog<T>:ILogWriter<T>,IDisposable
    {
        
    }
}
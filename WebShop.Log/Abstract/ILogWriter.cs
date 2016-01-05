using System;
using System.Net;

namespace WebShop.Log.Abstract
{
    public interface ILogWriter<in T>
    {
        void LogWriteInfo(T message);
        void LogWriteWrong(T message);
        void LogWriteError(T message, Exception exception);
    }
}
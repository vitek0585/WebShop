using System.Collections.Generic;

namespace WebShop.Log.Abstract
{
    public interface ILogReader<out TResult>
    {

        IEnumerable<TResult> LogRead();
    }
}
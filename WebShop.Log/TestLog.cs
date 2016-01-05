using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using NUnit.Framework;
using WebShop.Log.Concreate;

namespace WebShop.Log
{
    [TestFixture]
    public class TestLog
    {
        [Test]
        public async void WriteLog()
        {
            List<Task> list = new List<Task>();
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""), 
                new HttpResponse(new StringWriter()));
            for (int i = 0; i < 1; i++)
            {
                var t = Task.Factory.StartNew(Execute);
                list.Add(t);

            }
            await Task.WhenAll(list);
        }

        private void Execute()
        {
            for (int i = 0; i < 100; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    LogSql log = new LogSql();
                    log.LogWriteInfo("test message");
                    log.Dispose();
                    
                }, TaskCreationOptions.AttachedToParent);

            }
        }

        [Test]
        public void TestLogSql()
        {
           
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""),
                new HttpResponse(new StringWriter()));
            
            LogSql log = new LogSql();
            log.LogWriteInfo("TestLogSql message");
            log.Dispose();
        }
        [Test]
        public void TestLogExplicitDispose()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""),
                new HttpResponse(new StringWriter()));

            LogSql log = new LogSql();
            log.LogWriteInfo("TestLogSql message");
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        [Test]
        public void TestLogImplicitDispose()
        {
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://test.org", ""),
                new HttpResponse(new StringWriter()));

            LogSql log = new LogSql();
            log.LogWriteInfo("TestLogSql message");
            log.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
       
    }
}
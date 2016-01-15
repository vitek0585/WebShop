using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using WebShop.EFModel.Context;

namespace WebShop.EFModel.Test
{
    [TestFixture]
    public class TestDb
    {
        [Test]
        public void TestConnection()
        {
            var context = new ShopContext();
            var color = context.Colors;
            Assert.AreEqual(7, color.Count());
            Assert.AreEqual("red",color.FirstOrDefault().ColorNameEn);
        }

        
    }
}
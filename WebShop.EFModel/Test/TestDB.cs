using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using WebShop.EFModel.Context;
using WebShop.EFModel.Model;

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
            Assert.AreEqual("red", color.FirstOrDefault().ColorNameEn);
        }
        [Test]
        public void TestSale()
        {

            var context = new ShopContext();
            using (var t = context.Database.BeginTransaction())
            {
                try
                {
                    context.Sales.Add(new Sale()
                    {
                        DateSale = DateTime.Now,

                    });
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    t.Commit();
                    //t.Rollback();
                    
                }
            }
        }
        [Test]
        public void TestSalePos()
        {

            var context = new ShopContext();
            using (var t = context.Database.BeginTransaction())
            {
                try
                {
                    var s = new Sale()
                    {
                        DateSale = DateTime.Now,

                    };
                    var d=context.Sales.First();
                    var pos = new SalePos()
                    {
                        ClassificationId = 2050,
                        
                        
                    };
                   
                    d.SalePos.Add(pos);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Assert.Fail(e.InnerException.InnerException.Message);
                    Console.WriteLine(e);
                }
                finally
                {
                    t.Rollback();

                }
            }
        }

    }
}
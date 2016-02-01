using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebShop.ProxyMap
{
    public interface INum
    {
         int Number { get; set; }
    }
    public interface ITest
    {
        string UserName { get; set; }
        IEnumerable<INum> List { get; set; }
    }
    [TestFixture]
    public class GenerateProxy
    {

        [Test]
        public void Test()
        {
            Expression<Func<int>> e = () => 1;
            var t = e.Compile();
            var d = new
            {
                UserName = "Ivan",
                list = Enumerable.Repeat(new {Number=100},10)
            }; 
            var instance = CreateType<ITest>(d);

            Assert.That(instance.UserName, Is.EqualTo("Ivan"));
            Assert.That(instance.List.Count(), Is.EqualTo(10));
        }

        public T CreateType<T>(object obj)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            string currentAss = Assembly.GetExecutingAssembly().FullName;
            AssemblyName name = new AssemblyName(currentAss);

            AssemblyBuilder builder = domain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder mb = builder.DefineDynamicModule("ProxyModule");

            var typeName = string.Concat("Proxy", typeof(T).Name);
            TypeBuilder type = mb.DefineType(typeName, TypeAttributes.Public);
            type.AddInterfaceImplementation(typeof(T));
            var propertiesOfT = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertiesOfD = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance) as PropertyInfo[];
            foreach (var p in propertiesOfT)
            {
                CreateProp(p, type);
            }
            var common = propertiesOfT.Intersect(propertiesOfD, new PropertyComparer());
            var resultType = type.CreateType();
            var instance = Activator.CreateInstance(resultType);
            foreach (var p in common)
            {
                var pd = obj.GetType().GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).GetValue(obj,null);

                typeof(T).GetProperty(p.Name, BindingFlags.Public | BindingFlags.Instance).SetValue(instance, pd);
            }
            return (T)instance;
        }


        private class PropertyComparer : IEqualityComparer<PropertyInfo>
        {
            public bool Equals(PropertyInfo x, PropertyInfo y)
            {
                return string.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase)
                    && x.PropertyType == y.PropertyType;
            }

            public int GetHashCode(PropertyInfo obj)
            {
                return 0;
            }
        }
        private static void CreateProp(PropertyInfo pi, TypeBuilder type)
        {
            var nameProp = pi.Name;
            var typeProp = pi.PropertyType;
            FieldBuilder field = type.DefineField(string.Concat("_", nameProp), typeProp, FieldAttributes.Private);

            PropertyBuilder prop = type.DefineProperty(nameProp, PropertyAttributes.None, typeProp, Type.EmptyTypes);

            MethodBuilder getter = type.DefineMethod(string.Concat("get_", nameProp),
                MethodAttributes.Public | MethodAttributes.Virtual, typeProp, Type.EmptyTypes);
            ILGenerator il = getter.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            il.Emit(OpCodes.Ret);
            prop.SetGetMethod(getter);

            MethodBuilder setter = type.DefineMethod(string.Concat("set_", nameProp),
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.Virtual,
                typeof(void), new Type[] { typeProp });
            il = setter.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
            prop.SetSetMethod(setter);
        }

    }
}

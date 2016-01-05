using System;

namespace WebShop.Infostructure.Extenssions
{
    public static class StringExtenssion
    {
        public static string F(this string str, params object[] arg)
        {
            return String.Format(str, arg);
        }

    }
}
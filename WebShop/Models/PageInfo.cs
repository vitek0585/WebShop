using System;
using System.Collections.Generic;
using System.Linq;

namespace WebShop.Models
{
    public class PageInfo<T>
    {

        public byte TotalPerPage { get; set; }
        public short TotalPages { get; set; }
        public short CurrentPage { get; set; }
        public IEnumerable<object> Items { get; set; }
        private Func<T,object> _filter { get; set; }

        public PageInfo(double count, byte totalPerPage,short currentPage)
        {
            TotalPerPage = totalPerPage;
            CurrentPage = currentPage;
            SetTotalPage(count);

        }

        public PageInfo(IEnumerable<T> items,Func<T,object> filter, byte perPage, short currentPage)
        {
            _filter = filter;
            Items = GetUsersByPage(items, currentPage, perPage);
            TotalPerPage = perPage;
            CurrentPage = currentPage;
            SetTotalPage(items.Count());

        }

        public IEnumerable<object> GetUsersByPage(IEnumerable<T> items, short page, short perPage)
        {
            return items.Skip(perPage * (page - 1)).Take(perPage).Select(_filter);
            
        }

        private void SetTotalPage(double total)
        {
            TotalPages = (short)Math.Ceiling(total / TotalPerPage);
        }
    }
}
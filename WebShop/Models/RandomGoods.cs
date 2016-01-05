using System;
using System.Collections.Generic;
using System.Linq;
using WebShop.Repo.Interfaces;

namespace WebShop.Models
{
    public class RandomGoods
    {
        private int _count;

        public RandomGoods(int count)
        {
            _count = count;
        }
        public IEnumerable<T> GetRandomGoods<T>(IGlobalRepository<T> repository, int randomSize) where T : class
        {
            var rand  = new Random().Next(0, _count - randomSize + 1);
            return repository.GetAll().Skip(rand).Take(randomSize);
        }

    }
}
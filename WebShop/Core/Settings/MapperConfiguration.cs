using System.Linq;
using AutoMapper;
using WebShop.EFModel.Model;
using WebShop.Models;

namespace WebShop.Core.Settings
{
    public class MapperConfiguration
    {
        private static int? GetPhoto(Good good)
        {
            return good.Image.Any() ? good.Image.FirstOrDefault().ImageId : default(int?);
        }
        public static void ConfigSetup()
        {
            Mapper.CreateMap<UserOrder, SalePos>().
                ForMember(s => s.Price, opt => opt.MapFrom(g => g.PriceUsd)).
                ForMember(s => s.CountGood, opt => opt.MapFrom(g => g.CountGood));

            Mapper.CreateMap<GoodsWebApi, Good>();

            Mapper.CreateMap<Good, GoodHome>().ForMember(d => d.PhotoPath, opt => opt.MapFrom(g => GetPhoto(g))).
                ForMember(g=>g.GoodCount,opt=>opt.MapFrom(o=>o.ClassificationGoods.Sum(c=>c.CountGood)));

        }

    }
}
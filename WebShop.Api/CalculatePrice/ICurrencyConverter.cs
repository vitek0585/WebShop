namespace WebShop.Api.CalculatePrice
{
    public interface ICurrencyConverter
    {
        decimal ConvertUsdTo(decimal value, string convertTo);
        decimal ConvertWithCeiling(decimal value, string convertTo);
    }
}
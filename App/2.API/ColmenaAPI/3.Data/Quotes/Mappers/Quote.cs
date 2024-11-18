using Entities.Colmena.NuGet;

namespace Data.Quotes.Colmena.Mappers
{
    internal static class Mapper
    {
        internal static Quote Map(this object[] item)
        {
            if(item == null)
            {
                return new Quote();
            }

            return new Quote
            {
                QuoteId = Convert.ToInt32(item[0]),
                ProductId = Convert.ToInt32(item[1]),
                Product = new()
                {
                    { "productId" , Convert.ToInt32(item[1]) },
                    { "productName" , Convert.ToString(item[2]) },
                    { "lineId" , Convert.ToInt32(item[3]) },
                    { "lineName" , Convert.ToString(item[4]) }
                },
                
                UserId = Convert.ToInt32(item[5]),
                User = new()
                {
                    { "userId" , Convert.ToInt32(item[5]) },
                    { "name" , Convert.ToString(item[6]) },
                },

                QuoteDate = Convert.ToDateTime(item[7]),
                Total = Convert.ToDecimal(item[8]),
                StateId = Convert.ToInt32(item[9]),
                UserAdd = Convert.ToInt32(item[10]),
                DateAdd = Convert.ToDateTime(item[11]),
                UserMod = (item[12] is DBNull) ? null : Convert.ToInt32(item[12]),
                DateMod = (item[13] is DBNull) ? null : Convert.ToDateTime(item[13])
            };
        }

        internal static List<Quote> Map(this List<Object[]> items)
        {
            return items.Select(x => x.Map()).ToList();
        }
    }
}


using Entities.Colmena.NuGet;

namespace Data.Products.Colmena.Mappers
{
    internal static class Mapper
    {
        internal static Product Map(this object[] item)
        {
            if(item == null)
            {
                return new Product();
            }

            return new Product
            {
                ProductId = Convert.ToInt32(item[0]),
                LineId = Convert.ToInt32(item[1]),
                Line = new()
                {
                    { "lineId" , Convert.ToInt32(item[1]) },
                    { "name" , Convert.ToString(item[2]) }
                },
                Name = Convert.ToString(item[3]),
                Details = Convert.ToString(item[4]),
                BasePrice = Convert.ToDecimal(item[5]),
                StateId = Convert.ToInt32(item[6]),
                UserAdd = Convert.ToInt32(item[7]),
                DateAdd = Convert.ToDateTime(item[8]),
                UserMod = (item[9] is DBNull) ? null : Convert.ToInt32(item[9]),
                DateMod = (item[10] is DBNull) ? null : Convert.ToDateTime(item[10])
            };
        }

        internal static List<Product> Map(this List<Object[]> items)
        {
            return items.Select(x => x.Map()).ToList();
        }
    }
}


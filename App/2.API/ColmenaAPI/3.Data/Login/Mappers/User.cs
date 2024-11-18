using Entities.Colmena.NuGet;

namespace Data.Login.Colmena.Mappers
{
    internal static class Mapper
    {
        internal static User Map(this object[] item)
        {
            if(item == null)
            {
                return new User();
            }

            return new User
            {
                UserId = Convert.ToInt32(item[0]),
                UserTypeId = Convert.ToInt32(item[1]),
                Names = Convert.ToString(item[2]),
                Lastnames = Convert.ToString(item[3]),
                Email = Convert.ToString(item[4]),
                Password = Convert.ToString(item[5]),
                Environment = Convert.ToString(item[6]),
                StateId = Convert.ToInt32(item[7]),
                UserAdd = Convert.ToInt32(item[8]),
                DateAdd = Convert.ToDateTime(item[9]),
                UserMod = (item[10] is DBNull) ? null : Convert.ToInt32(item[10]),
                DateMod = (item[11] is DBNull) ? null : Convert.ToDateTime(item[11])
            };
        }

        internal static List<User> Map(this List<Object[]> items)
        {
            return items.Select(x => x.Map()).ToList();
        }
    }
}


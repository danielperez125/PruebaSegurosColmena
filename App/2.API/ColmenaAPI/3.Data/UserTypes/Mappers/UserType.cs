using Entities.Colmena.NuGet;

namespace Data.UserTypes.Colmena.Mappers
{
    internal static class Mapper
    {
        internal static UserType Map(this object[] item)
        {
            if(item == null)
            {
                return new UserType();
            }

            return new UserType
            {
                UserTypeId = Convert.ToInt32(item[0]),
                Name = Convert.ToString(item[1]),
                Details = Convert.ToString(item[2]),
                StateId = Convert.ToInt32(item[3]),
            };
        }

        internal static List<UserType> Map(this List<Object[]> items)
        {
            return items.Select(x => x.Map()).ToList();
        }
    }
}


using System.Collections.Generic;
using System.Linq;
using Shsict.Peccancy.Service.DbHelper;
using Shsict.Peccancy.Service.Model;

namespace Shsict.Peccancy.Service.Extension
{
    public static class RepositoryExtensions
    {
        public static int Insert<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var enumerable = source as T[] ?? source.ToArray();

            if (enumerable.Any())
            {
                using (IRepository repo = new Repository())
                {
                    return repo.Insert(enumerable);
                }
            }

            return 0;
        }

        public static int Update<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var enumerable = source as T[] ?? source.ToArray();

            if (enumerable.Any())
            {
                using (IRepository repo = new Repository())
                {
                    return repo.Save(enumerable);
                }
            }

            return 0;
        }

        public static int Delete<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var enumerable = source as T[] ?? source.ToArray();

            if (enumerable.Any())
            {
                using (IRepository repo = new Repository())
                {
                    return repo.Delete(enumerable);
                }
            }

            return 0;
        }
    }
}
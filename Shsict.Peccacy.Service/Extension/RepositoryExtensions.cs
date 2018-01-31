using System.Collections.Generic;
using System.Linq;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service.Extension
{
    public static class RepositoryExtensions
    {
        public static int Insert<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            var ret = 0;

            if (list.Count > 0)
            {
                using (IRepository repo = new Repository())
                {
                    ret += list.Sum(instance => repo.Insert((IEntity)instance));
                }
            }

            return ret;
        }

        public static int Update<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            var ret = 0;

            if (list.Count > 0)
            {
                using (IRepository repo = new Repository())
                {
                    ret = repo.Save(list.ToArray());
                }
            }

            return ret;
        }

        public static int Delete<T>(this IEnumerable<T> source) where T : class, IEntity
        {
            var list = source as IList<T> ?? source.ToList();

            var ret = 0;

            if (list.Count > 0)
            {
                using (IRepository repo = new Repository())
                {
                    ret += list.Sum(instance => repo.Delete((IEntity)instance));
                }
            }

            return ret;
        }
    }
}
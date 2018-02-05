using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void Test_Single()
        {
            using (IRepository repo = new Repository())
            {
                // correct value of argument
                var key1 = 2;

                var instance1 = repo.Single<Config>(key1);

                Assert.IsNotNull(instance1);
                Assert.IsInstanceOfType(instance1, typeof(Config));

                // wrong value of argument
                var key2 = 0;

                var instance2 = repo.Single<Config>(key2);

                Assert.IsNull(instance2);
            }
        }

        [TestMethod]
        public void Test_All()
        {
            using (IRepository repo = new Repository())
            {
                var query = repo.All<Config>();

                Assert.IsNotNull(query);
                Assert.IsInstanceOfType(query, typeof(List<Config>));
                Assert.IsTrue(query.Any());

                Assert.IsTrue(repo.Count<Config>(x => x.ConfigKey.Contains("Assembly")) > 0);
            }
        }

        [TestMethod]
        public void Test_Query()
        {
            using (IRepository repo = new Repository()
            )
            {
                // correct value of argument
                // ReSharper disable once RedundantBoolCompare
                var query1 = repo.Query<Config>(x => x.ConfigKey.Equals("SystemActive"));

                Assert.IsNotNull(query1);
                Assert.IsInstanceOfType(query1, typeof(List<Config>));
                Assert.IsTrue(query1.Any());

                // wrong value of argument
                var query2 = repo.Query<Config>(x => x.ConfigValue.Equals("false true"));

                Assert.IsNotNull(query2);
                Assert.IsInstanceOfType(query2, typeof(List<Config>));
                Assert.IsFalse(query2.Any());
            }
        }

        [TestMethod]
        public void Test_Insert_Update_Delete()
        {
            var c = new Config
            {
                ConfigKey = "test",
                ConfigValue = DateTime.Now.ToShortDateString(),
            };

            using (IRepository repo = new Repository())
            {
                Assert.IsTrue(repo.Insert(c) > 0);

                var res = repo.Single<Config>(c.ID);

                Assert.IsNotNull(res);
                Assert.IsInstanceOfType(res, typeof(Config));

                res.ConfigValue = "modified";

                repo.Save(res);

                var resUpdated = repo.Single<Config>(res.ID);

                Assert.IsNotNull(resUpdated);
                Assert.IsInstanceOfType(resUpdated, typeof(Config));
                Assert.IsTrue(resUpdated.Equals(res));
                Assert.IsTrue(resUpdated.ConfigValue.Equals("modified"));

                var id = res.ID;

                repo.Delete(res);

                var resDeleted = repo.Single<Config>(id);

                Assert.IsNull(resDeleted);
            }
        }
    }
}
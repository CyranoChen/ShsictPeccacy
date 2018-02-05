using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Shsict.Peccacy.Service.DbHelper;
using Shsict.Peccacy.Service.Logger;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service.Tests
{
    [TestClass()]
    public class OracleDbContextTests
    {
        [TestMethod()]
        public void OracleDbContextTest()
        {
            using (var ctx = new OracleDbContext())
            {
                var trucks = ctx.TruckCamRecords.ToList();
                var sources = ctx.CameraSources.ToList();
                var schedules = ctx.Schedules.ToList();
            }

            using (var ctx = new OracleDbContext())
            {
                var configs = ctx.Configs.ToList();

                if (configs.Count > 0)
                {
                    var c = configs[0];
                    var key = c.ConfigKey;
                    var value = c.ConfigValue;

                    c.ConfigValue = "321";

                    ctx.SaveChanges();
                }
                else
                {
                    var c = new Config() { ConfigKey = "test", ConfigValue = "123456" };

                    ctx.Configs.Add(c);

                    ctx.SaveChanges();
                }

                IList list = ctx.Configs.ToList();
            }
        }

        [TestMethod()]
        public void LoggingTest()
        {
            var log = new DaoLog();
            log.Debug("test");

            log.Debug(new Exception("error"));

            using (IRepository repo = new Repository())
            {
                var list = repo.All<Log>();

                Assert.IsTrue(repo.Count<Log>(x => x.Logger == "DaoLog") > 0);
            }
        }
    }
}
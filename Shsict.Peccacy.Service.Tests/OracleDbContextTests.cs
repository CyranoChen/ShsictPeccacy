using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Shsict.Peccacy.Service.DbHelper;
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
                    var c = new Config() { ConfigKey = "test", ConfigValue = "123456"};

                    ctx.Configs.Add(c);

                    ctx.SaveChanges();
                }

                IList list = ctx.Configs.ToList();
            }
        }
    }
}
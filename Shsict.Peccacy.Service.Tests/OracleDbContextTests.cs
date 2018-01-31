using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shsict.Peccacy.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }


            Assert.Fail();
        }
    }
}
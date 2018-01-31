using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.OleDb;
using Shsict.Peccacy.Service.DbHelper;

namespace Shsict.Peccacy.Service.Tests
{
    [TestClass()]
    public class AccessDbTests
    {
        [TestMethod()]
        public void AccessDbNativeTest()
        {
            //连接Access字符串
            string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\SqlData\Peccacy.mdb;Persist Security Info=False";

            DataSet ds = new DataSet();
            var sql = "SELECT * FROM Truck_Record";

            using (var conn = new OleDbConnection(conStr))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                using (var adapter = new OleDbDataAdapter(sql, conn))
                {
                    adapter.Fill(ds);
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    var id = dr["ID"];
                    var truckNo = dr["TruckNo"];
                    var createTime = dr["CreateTime"];
                    var remark = dr["Remark"];
                }
            }
        }

        [TestMethod()]
        public void AccessDbHelperTest()
        {
            const string conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\SqlData\Peccacy.mdb;Persist Security Info=False";

            var sql =
                @"SELECT * FROM [Truck_Record]";

            var ds = AccessHelper.ExecuteDataset(conStr, CommandType.Text, sql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                Assert.IsTrue(dt.Rows.Count > 0);
            }
        }
    }
}

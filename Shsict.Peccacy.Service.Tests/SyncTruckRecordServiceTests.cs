using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Shsict.Peccacy.Service.Model;

namespace Shsict.Peccacy.Service.Tests
{
    [TestClass()]
    public class SyncTruckRecordServiceTests
    {
        [TestMethod()]
        public void SyncCameraSourceTest()
        {
            var cam = CameraSource.Cache.CameraSourceList.FirstOrDefault(x => x.IsSync);

            if (cam != null)
            {
                SyncTruckRecordService.SyncCameraSource(cam);
            }
        }
    }
}
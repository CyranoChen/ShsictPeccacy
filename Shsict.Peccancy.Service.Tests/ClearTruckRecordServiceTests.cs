using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Shsict.Peccancy.Service.Model;

namespace Shsict.Peccancy.Service.Tests
{
    [TestClass()]
    public class ClearTruckRecordServiceTests
    {
        [TestMethod()]
        public void ClearCameraSourceTest()
        {
            var cam = CameraSource.Cache.CameraSourceList.FirstOrDefault(x => x.CamNo == "81#");

            if (cam != null)
            {
                ServiceTruckRecord.ClearCameraSource(cam);
            }
        }
    }
}
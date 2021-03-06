﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Shsict.Peccancy.Service.Model;

namespace Shsict.Peccancy.Service.Tests
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
                ServiceTruckRecord.SyncCameraSource(cam);
            }
        }
    }
}
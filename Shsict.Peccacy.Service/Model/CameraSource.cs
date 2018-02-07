using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Shsict.Peccacy.Service.DbHelper;

namespace Shsict.Peccacy.Service.Model
{
    [Table("PECCACY_CAMERASOURCE")]
    public class CameraSource : Entity<int>
    {
        public CameraSource() { }

        #region Members and Properties

        [Column("CAMNO")]
        public string CamNo { get; set; }

        [Column("ISSYNC")]
        public bool IsSync { get; set; }

        [Column("LASTSYNCTIME")]
        public DateTime LastSyncTime { get; set; }

        [Column("CONNSTRING")]
        public string ConnString { get; set; }

        [Column("VIEWNAME")]
        public string ViewName { get; set; }

        [Column("REMARK")]
        public string Remark { get; set; }

        #endregion


        public static class Cache
        {
            public static List<CameraSource> CameraSourceList;

            static Cache()
            {
                InitCache();
            }

            public static void RefreshCache()
            {
                InitCache();
            }

            private static void InitCache()
            {
                using (IRepository repo = new Repository())
                {
                    CameraSourceList = repo.All<CameraSource>();
                }
            }

            public static CameraSource Load(int id)
            {
                return CameraSourceList.Find(x => x.ID == id);
            }
        }
    }
}
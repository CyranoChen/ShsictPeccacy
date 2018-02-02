using System;
using System.ComponentModel.DataAnnotations.Schema;

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

    }
}
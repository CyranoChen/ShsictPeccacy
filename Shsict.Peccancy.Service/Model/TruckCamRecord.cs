using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Shsict.Peccancy.Service.Model
{
    [Table("SSICT_PECCANCY_TRUCKCAMRECORD")]
    public class TruckCamRecord : Entity<int>
    {
        public TruckCamRecord() { }

        public TruckCamRecord(DataRow dr, string camNo)
        {
            CamNo = camNo;
            License = dr["TRUCKLICENSE"].ToString();
            Speed = Convert.ToInt16(dr["SPEED"]);
            PicTime = Convert.ToDateTime(dr["PICTIME"]);
            PicName = dr["PICNAME"].ToString();
        }

        #region Members and Properties

        [Column("CAMNO")]
        public string CamNo { get; set; }

        [Column("LICENSE")]
        public string License { get; set; }

        [Column("SPEED")]
        public int Speed { get; set; }

        [Column("PICTIME")]
        public DateTime PicTime { get; set; }

        [Column("PICNAME")]
        public string PicName { get; set; }

        #endregion
    }
}

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shsict.Peccacy.Service.Model
{
    [Table("PECCACY_TRUCKCAMRECORD")]
    public class TruckCamRecord : Entity<int>
    {
        public TruckCamRecord() { }

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

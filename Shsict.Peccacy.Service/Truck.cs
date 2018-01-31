using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shsict.Peccacy.Service
{
    [Table("Truck_Record")]
    public class Truck
    {
        #region Members and Properties

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        public string TruckNo { get; set; }

        public DateTime CreateTime { get; set; }

        public string Remark { get; set; }

        #endregion

    }
}

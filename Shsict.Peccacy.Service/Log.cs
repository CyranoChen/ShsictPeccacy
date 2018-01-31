using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shsict.Peccacy.Service
{
    /// <summary>
    /// Log
    /// </summary>
    [Table("Peccacy_Log")]
    public class Log
    {
        #region Members and Properties

        [Key]
        [Column("Id")]
        public int Id { get; set; }

        public string Logger { get; set; }

        public DateTime CreateTime { get; set; }

        public string Message { get; set; }

        public bool IsException { get; set; }

        public string StackTrace { get; set; }

        #endregion
    }
}

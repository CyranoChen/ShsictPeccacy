using System.Configuration;
using System.Data.Entity;
using Shsict.Peccacy.Service.Logger;
using Shsict.Peccacy.Service.Model;
using Shsict.Peccacy.Service.Scheduler;

namespace Shsict.Peccacy.Service.DbHelper
{
    /// <summary>
    /// OracleDbContext
    /// </summary>
    public class OracleDbContext : DbContext
    {
        /// <summary>
        /// OracleDbContext
        /// </summary>
        public OracleDbContext() : base("DataDestination") { }

        /// <summary>
        /// TruckCamRecord
        /// </summary>
        public DbSet<TruckCamRecord> TruckCamRecords { get; set; }

        /// <summary>
        /// CameraSource
        /// </summary>
        public DbSet<CameraSource> CameraSources { get; set; }

        /// <summary>
        /// Config
        /// </summary>
        public DbSet<Config> Configs { get; set; }

        /// <summary>
        /// Schedule
        /// </summary>
        public DbSet<Schedule> Schedules { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        public DbSet<Log> Logs { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var owner = ConfigurationManager.AppSettings["Oracle.Schema.Owner"];
            modelBuilder.HasDefaultSchema(owner.ToUpper());
        }
    }
}

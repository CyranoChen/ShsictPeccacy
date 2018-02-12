using System.Configuration;
using System.Data.Entity;
using Shsict.Peccancy.Service.Logger;
using Shsict.Peccancy.Service.Model;
using Shsict.Peccancy.Service.Scheduler;

namespace Shsict.Peccancy.Service.DbHelper
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

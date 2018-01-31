using System.Data.Entity;
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

        ///// <summary>
        ///// Truck
        ///// </summary>
        //public DbSet<Truck> Truck { get; set; }

        /// <summary>
        /// Config
        /// </summary>
        public DbSet<Config> Configs { get; set; }

        /// <summary>
        /// Scheduler
        /// </summary>
        public DbSet<Schedule> Schedulers { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        //public DbSet<Log> Logs { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SHSICT");
        }
    }
}

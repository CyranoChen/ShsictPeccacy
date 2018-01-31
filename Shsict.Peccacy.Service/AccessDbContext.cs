using System.Data.Entity;

namespace Shsict.Peccacy.Service
{
    /// <summary>
    /// OracleDbContext
    /// </summary>
    public class AccessDbContext : DbContext
    {
        /// <summary>
        /// OracleDbContext
        /// </summary>
        public AccessDbContext() : base("DataSource") { }

        /// <summary>
        /// Truck
        /// </summary>
        public DbSet<Truck> Truck { get; set; }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("PECCACY");
        }
    }
}

using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using ArkaChart.Domain.Mapping.Entities;

namespace ArkaChart.Domain.Mapping.Context
{
    public class EntitiesContext : DbContext
    {
        public EntitiesContext(DbConnection connection)
            : base(connection, true)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.AutoDetectChangesEnabled = false;
        }
        public DbSet<DataFile> Files { get; set; }
        public DbSet<DataLine> Lines { get; set; }

    }
}
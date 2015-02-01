using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;

namespace ArkaChart.Domain.Mapping.Context
{
    public class EntityObjectContext : IDbContext
    {
        private readonly EntitiesContext _context;

        public EntityObjectContext()
        {
            _context = new EntitiesContext(GetConnection());
        }

        private static DbConnection GetConnection() {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        public IDbSet<T> CreateObjectSet<T>() where T : class
        {
            return _context.Set<T>();
        }

        public EntitiesContext GetContext()
        {
            return _context;
        }

        public IDbEntityEntryContext GetEntry(object entity)
        {
            return new DbEntityEntryContext(GetContext().Entry(entity));;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }
    }
}
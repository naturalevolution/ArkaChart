using System;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using ArkaChart.Domain.Mapping.Context;

namespace ArkaChart.Tests.Data.Context {
    public class EntityObjectContextTest : IDbContext {
        private readonly EntitiesContext _context;

        public EntityObjectContextTest() {
            _context = new EntitiesContext(GetConnection());
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDbSet<T> CreateObjectSet<T>() where T : class {
            return _context.Set<T>();
        }

        public EntitiesContext GetContext() {
            return _context;
        }

        public IDbEntityEntryContext GetEntry(object entity) {
            return new DbEntityEntryContext(GetContext().Entry(entity));
            ;
        }
        private static DbConnection GetConnection() {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnectionTest"].ConnectionString);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (_context != null) {
                    _context.Dispose();
                }
            }
        }
    }
}
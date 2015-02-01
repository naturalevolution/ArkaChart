using System.Data;
using System.Data.Entity.Infrastructure;

namespace ArkaChart.Domain.Mapping.Context
{
    public class DbEntityEntryContext : IDbEntityEntryContext
    {
        private DbEntityEntry _entry;

        public DbEntityEntryContext(DbEntityEntry entry)
        {
            _entry = entry;
        }

        public bool IsState(EntityState state)
        {
            if (_entry.State == state)
            {
                return true;
            }
            return false;
        }

        public void SetState(EntityState state)
        {
            _entry.State = state;
        }
    }
}
using System.Data;

namespace ArkaChart.Domain.Mapping.Context
{
    public interface IDbEntityEntryContext
    {
        bool IsState(EntityState state);
        void SetState(EntityState state);
    }
}
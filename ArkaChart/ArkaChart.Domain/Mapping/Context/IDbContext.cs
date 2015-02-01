using System;
using System.Data.Entity;

namespace ArkaChart.Domain.Mapping.Context {
    public interface IDbContext : IDisposable {
        IDbSet<T> CreateObjectSet<T>() where T : class;
        EntitiesContext GetContext();
        IDbEntityEntryContext GetEntry(object entity);
    }
}
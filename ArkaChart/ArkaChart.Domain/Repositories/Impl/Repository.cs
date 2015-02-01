using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ArkaChart.Domain.Mapping.Context;

namespace ArkaChart.Domain.Repositories.Impl {
    public class Repository<T> : IRepository<T> where T : class {
        protected IDbContext _objectContext;
        protected IDbSet<T> _objectSet;

        public Repository(IDbContext objectContext) {
            _objectContext = objectContext;
            _objectSet = _objectContext.CreateObjectSet<T>();
        }
        public virtual IList<T> FindAll() {
            return _objectSet.ToList();
        }

        public virtual IList<T> FindBy(Expression<Func<T, bool>> predicate) {
            return _objectSet.Where(predicate).ToList();
        }

        public virtual T FindDistinctBy(Expression<Func<T, bool>> predicate) {
            return FindBy(predicate).FirstOrDefault();
        }

        public virtual void Add(T entity) {
            IDbEntityEntryContext entry = _objectContext.GetEntry(entity);
            if (entry.IsState(EntityState.Detached)) {
                _objectSet.Add(entity);
            } else {
                entry.SetState(EntityState.Added);
            }
        }

        public virtual void Update(T entity) {
            IDbEntityEntryContext entry = _objectContext.GetEntry(entity);
            if (entry.IsState(EntityState.Detached)) {
                _objectSet.Attach(entity);
            } else {
                entry.SetState(EntityState.Modified);
            }
        }

        public virtual void Remove(T entity) {
            IDbEntityEntryContext entry = _objectContext.GetEntry(entity);
            if (entry.IsState(EntityState.Detached)) {
                _objectSet.Attach(entity);
            } else {
                entry.SetState(EntityState.Deleted);
            }
            _objectSet.Remove(entity);
        }
    }
}
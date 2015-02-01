using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ArkaChart.Domain.Repositories {
    public interface IRepository<T> where T : class
	{
		IList<T> FindAll();
		IList<T> FindBy(Expression<Func<T, bool>> predicate);
		T FindDistinctBy(Expression<Func<T, bool>> predicate);
		void Add(T newEntity);
		void Remove(T entity);
		void Update(T entity);
	}
}
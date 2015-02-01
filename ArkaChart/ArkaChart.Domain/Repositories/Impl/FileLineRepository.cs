using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ArkaChart.Domain.Mapping.Context;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Domain.Mapping.Entities.Beans;
using ArkaChart.Tools;

namespace ArkaChart.Domain.Repositories.Impl {
    public class FileLineRepository : Repository<DataLine>, IFileLineRepository {
        public FileLineRepository(IDbContext objectContext)
            : base(objectContext) {}

        public List<SumValueDateBean> CalculateSumByDate() {
            return  _objectContext.GetContext().Lines.GroupBy(x => new {x.Timestamp.Day, x.Timestamp.Month, x.Timestamp.Year})
                    .Select(x => new SumValueDateBean { Year = x.Key.Year, Month = x.Key.Month, Day = x.Key.Day, Value = x.Sum(s => s.Value) })
                    .ToList();
        }

        public List<SumValueOriginBean> CalculateSumByOrigin() {
            return _objectContext.GetContext().Lines.GroupBy(x => new { x.Timestamp.Day, x.Timestamp.Month, x.Timestamp.Year, x.Origin })
                    .Select(x => new SumValueOriginBean { Source = x.Key.Origin, Dates = new SumValueDateBean { Year = x.Key.Year, Month = x.Key.Month, Day = x.Key.Day, Value = x.Sum(s => s.Value) } })
                    .OrderBy(g => g.Source)
                    .ToList();
        }
    }
}
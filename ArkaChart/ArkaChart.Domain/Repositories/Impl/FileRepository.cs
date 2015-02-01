using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ArkaChart.Domain.Mapping.Context;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Domain.Mapping.Entities.Beans;
using ArkaChart.Tools;

namespace ArkaChart.Domain.Repositories.Impl {
    public class FileRepository : Repository<DataFile>, IFileRepository {
        public FileRepository(IDbContext objectContext)
            : base(objectContext) {}

        public List<DataFile> ProcessingFiles() {
            return FindBy(x => x.Status == (int) StatusHelper.Processing ||
                               x.Status == (int) StatusHelper.Waiting).ToList();
        }

        public List<SumValueStringBean> CalculateSumOfAllValues() {
            return _objectContext.GetContext().Files.Where(x => x.Lines.Count > 0).Select(lg => new SumValueStringBean { Source = lg.Name, Value = lg.Lines.Sum(x => x.Value) }).ToList();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Domain.Mapping.Entities.Beans;

namespace ArkaChart.Domain.Repositories {
    public interface IFileRepository : IRepository<DataFile> {
        List<DataFile> ProcessingFiles();

        List<SumValueStringBean> CalculateSumOfAllValues();
    }
}
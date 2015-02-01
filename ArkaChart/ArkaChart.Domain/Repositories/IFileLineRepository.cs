using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArkaChart.Domain.Mapping.Entities;
using ArkaChart.Domain.Mapping.Entities.Beans;

namespace ArkaChart.Domain.Repositories {
    public interface IFileLineRepository : IRepository<DataLine> {
        List<SumValueDateBean> CalculateSumByDate();

        List<SumValueOriginBean> CalculateSumByOrigin();
    }
}
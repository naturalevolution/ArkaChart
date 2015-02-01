using System.Collections.Generic;
using ArkaChart.Controllers;
using ArkaChart.Domain.Mapping.Entities.Beans;

namespace ArkaChart.Models {
    public class DatasModel {
        public List<SumValueStringBean> SumOfValues { get; set; }
        public SumAndYearModel SumOfYear { get; set; }
        public SumAndOriginModel SumOfOrigin { get; set; }
    }
}
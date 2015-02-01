using System;
using System.Collections.Generic;

namespace ArkaChart.Domain.Mapping.Entities.Beans {
    public abstract class SumValueBean {
        public int Value { get; set; }
    }
    public class SumValueStringBean : SumValueBean {
        public string Source { get; set; }
    }

    public class SumValueDateBean : SumValueBean {
        public string GetDate() {
            return (new DateTime(Year, Month, Day)).ToShortDateString();
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }

    public class SumValueOriginBean {
        public string Source { get; set; }
        public SumValueDateBean Dates { get; set; }
    }
}
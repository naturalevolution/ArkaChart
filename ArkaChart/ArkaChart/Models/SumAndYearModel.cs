using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArkaChart.Domain.Mapping.Entities.Beans;

namespace ArkaChart.Models {
    public class SumAndYearModel  {
        public SumAndYearModel(List<SumValueDateBean> elements) {
            SumsOfValues = elements;
        }

        public List<SumValueDateBean> SumsOfValues { get; set; }

        public string GetX() {
            var sb = new StringBuilder();
            var list = SumsOfValues.Select(x => x.GetDate());
            foreach (var element in list) {
                sb.Append("'"+element+"'").Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
        public string GetY() {
            var sb = new StringBuilder();
            sb.Append("{name:'Valeurs', data:[");
            var list = SumsOfValues.Select(x => x.Value);
            foreach (var element in list) {
                sb.Append(element).Append(",");
            }
            return sb.ToString().TrimEnd(',') + "]}";
        }
    }
}
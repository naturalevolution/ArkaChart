using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArkaChart.Domain.Mapping.Entities.Beans;

namespace ArkaChart.Models {
    public class SumAndOriginModel {
        public SumAndOriginModel(List<SumValueOriginBean> values) {
            SumsOfValues = values;
            DateValues = SumsOfValues.GroupBy(x => new { x.Dates.Day, x.Dates.Month, x.Dates.Year })
                                    .Select(x => new SumValueDateBean { Year = x.Key.Year, Month = x.Key.Month, Day = x.Key.Day })
                                    .ToList();
            
        }

        public List<SumValueDateBean> DateValues { get; set; }
        public List<SumValueOriginBean> SumsOfValues { get; set; }

        public string GetX() {
            var sb = new StringBuilder();
            foreach (var element in DateValues) {
                sb.Append("'" + element.GetDate() + "'").Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
        public string GetY() {
            var sb = new StringBuilder();
            foreach (var origin in SumsOfValues.GroupBy(x => new {x.Source}).Select(x => x.Key.Source)) {
                sb.Append(GetSerie(origin)).Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }

        private string GetSerie(string origin) {
            var sb = new StringBuilder();
            sb.Append("{name:'")
                .Append(origin)
                .Append("', data:[")
                .Append(GetDatas(origin));
            return sb + "]}";
        }

        private string GetDatas(string origin) {
            var sb = new StringBuilder();
            var values = SumsOfValues.Where(x => x.Source == origin).GroupBy(x => new {x.Dates.Day, x.Dates.Month, x.Dates.Year, x.Dates.Value}).ToList();
            foreach (var value in values) {
                sb.Append(value.Key.Value).Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
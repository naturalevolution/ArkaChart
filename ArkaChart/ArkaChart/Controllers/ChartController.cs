using System.Web.Mvc;
using ArkaChart.Domain.Factory;
using ArkaChart.Models;

namespace ArkaChart.Controllers {
    public class ChartController : Controller {
        public ActionResult Index() {
            var datasModel = new DatasModel();
            datasModel.SumOfValues = Repositories.Files.CalculateSumOfAllValues();
            datasModel.SumOfYear = new SumAndYearModel(Repositories.FileLines.CalculateSumByDate());
            datasModel.SumOfOrigin = new SumAndOriginModel(Repositories.FileLines.CalculateSumByOrigin());
            return View(datasModel);
        }
    }
}
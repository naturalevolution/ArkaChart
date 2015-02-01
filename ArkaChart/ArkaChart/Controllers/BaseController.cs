using System;
using System.Web.Mvc;
using ArkaChart.Filters;
using ArkaChart.Tools;

namespace ArkaChart.Controllers {
    [OpenTransaction]
    public class BaseController : Controller {
        protected const string ERROR = "danger";
        protected const string SUCCESS = "success";
        protected const string WARNING = "warning";

        public PathHelper PathHelper = new PathHelper();
    }
}
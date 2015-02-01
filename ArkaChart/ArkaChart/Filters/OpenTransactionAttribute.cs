using System.Web.Mvc;
using ArkaChart.Domain.Factory;
using ArkaChart.Domain.Mapping.Context;

namespace ArkaChart.Filters {
    public class OpenTransactionAttribute : ActionFilterAttribute {
        private IDbContext _context;

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            _context = new EntityObjectContext();
            Repositories.LoadContext(_context);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            base.OnActionExecuted(filterContext);
            Repositories.SaveChanges();
        } 
    }
}
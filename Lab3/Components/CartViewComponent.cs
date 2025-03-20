using Microsoft.AspNetCore.Mvc;

namespace Lab3.Components
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

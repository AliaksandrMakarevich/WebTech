using Microsoft.AspNetCore.Mvc;

namespace Lab2.Components
{
    public class CartViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

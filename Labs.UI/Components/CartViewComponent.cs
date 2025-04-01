using Microsoft.AspNetCore.Mvc;

namespace Labs.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

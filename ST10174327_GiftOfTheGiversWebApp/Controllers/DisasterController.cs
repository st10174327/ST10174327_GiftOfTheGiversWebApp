using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize]
    public class DisasterController : Controller
    {
        public IActionResult Report()
        {
            return View();
        }
    }
}

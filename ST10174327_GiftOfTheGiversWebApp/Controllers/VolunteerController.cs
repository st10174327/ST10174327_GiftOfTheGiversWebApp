using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize]
    public class VolunteerController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize]
    public class DonateController : Controller
    {
        public IActionResult Goods()
        {
            return View();
        }

        public IActionResult Money()
        {
            return View();
        }
    }
}

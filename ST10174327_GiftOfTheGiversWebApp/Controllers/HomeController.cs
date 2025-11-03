using ST10174327_GiftOfTheGiversWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ST10174327_GiftOfTheGiversWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Landing page
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Admin users go to Admin Dashboard
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                // Regular authenticated users see donations + disasters
                var viewModel = new IncomingDataModel
                {
                    GoodsDonations = _context.GoodsDonation.ToList(),
                    MoneyDonations = _context.MoneyDonation.ToList(),
                    Disasters = _context.Disaster.ToList()
                };

                return View(viewModel);
            }

            // Guests see basic landing page
            return View();
        }

        // Admin dashboard redirection check
        public IActionResult AdminRedirect()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return RedirectToAction("Index");
        }

        // Static Pages
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        // Error Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

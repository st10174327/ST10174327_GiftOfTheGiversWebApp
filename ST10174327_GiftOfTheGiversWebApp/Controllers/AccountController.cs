using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.Security.Claims;
using ST10174327_GiftOfTheGiversWebApp.Models;
using Microsoft.Graph;

namespace ST10174327_GiftOfTheGiversWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly GraphServiceClient _graphServiceClient;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            GraphServiceClient graphServiceClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _graphServiceClient = graphServiceClient;
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AuthorizeForScopes(ScopeKeySection = "MicrosoftGraph:Scopes")]
        public async Task<IActionResult> Profile()
        {
            try
            {
                // Get user's profile from Microsoft Graph
                var user = await _graphServiceClient.Me.Request().GetAsync();
                ViewData["Photo"] = await GetUserPhoto();
                return View(user);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        private async Task<string> GetUserPhoto()
        {
            try
            {
                var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync();
                if (photoStream != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await photoStream.CopyToAsync(ms);
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch
            {
                // If there's no photo, we'll return null
            }
            return null;
        }
    }
}

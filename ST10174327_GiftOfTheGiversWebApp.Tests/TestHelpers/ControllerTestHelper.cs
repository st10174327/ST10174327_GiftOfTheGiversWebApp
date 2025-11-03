using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Security.Claims;
using System.Security.Principal;

namespace ST10174327_GiftOfTheGiversWebApp.Tests.TestHelpers
{
    public static class ControllerTestHelper
    {
        public static TController WithIdentity<TController>(
            this TController controller, 
            string userId = "test-user-id", 
            string username = "testuser@example.com",
            string role = "User") where TController : Controller
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }, "TestAuthentication"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        public static TController WithAnonymousIdentity<TController>(this TController controller) 
            where TController : Controller
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            };
            return controller;
        }

        public static void SetTempData<TController>(this TController controller, ITempDataDictionary tempData) 
            where TController : Controller
        {
            controller.TempData = tempData ?? new TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<ITempDataProvider>());
        }
    }
}

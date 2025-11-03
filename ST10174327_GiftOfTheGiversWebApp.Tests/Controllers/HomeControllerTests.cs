using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ST10174327_GiftOfTheGiversWebApp.Controllers;
using ST10174327_GiftOfTheGiversWebApp.Data;
using ST10174327_GiftOfTheGiversWebApp.Models;
using Xunit;

namespace ST10174327_GiftOfTheGiversWebApp.Tests.Controllers
{
    public class HomeControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _loggerMock = new Mock<ILogger<HomeController>>();
            
            // Setup UserManager mock
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            
            // Setup SignInManager mock
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object, 
                contextAccessor.Object, 
                userPrincipalFactory.Object, 
                null, null, null, null);

            _controller = new HomeController(
                _loggerMock.Object,
                _dbContext,
                _userManagerMock.Object,
                _signInManagerMock.Object);

            // Seed test data
            SeedTestData();
        }

        private void SeedTestData()
        {
            // Add test data to in-memory database
            if (!_dbContext.GoodsDonation.Any())
            {
                _dbContext.GoodsDonation.AddRange(new List<GoodsDonation>
                {
                    new GoodsDonation { Id = 1, Description = "Test Goods 1", Category = "Food", Quantity = 10 },
                    new GoodsDonation { Id = 2, Description = "Test Goods 2", Category = "Clothing", Quantity = 5 }
                });
            }

            if (!_dbContext.MoneyDonation.Any())
            {
                _dbContext.MoneyDonation.AddRange(new List<MoneyDonation>
                {
                    new MoneyDonation { Id = 1, Amount = 100.50m, DonationDate = DateTime.Now },
                    new MoneyDonation { Id = 2, Amount = 200.75m, DonationDate = DateTime.Now }
                });
            }

            if (!_dbContext.Disaster.Any())
            {
                _dbContext.Disaster.AddRange(new List<Disaster>
                {
                    new Disaster { Id = 1, Description = "Test Disaster 1", Location = "Test Location 1" },
                    new Disaster { Id = 2, Description = "Test Disaster 2", Location = "Test Location 2" }
                });
            }

            _dbContext.SaveChanges();
        }

        [Fact]
        public void Index_WhenUserIsNotAuthenticated_ReturnsViewResult()
        {
            // Arrange
            _controller.WithAnonymousIdentity();

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewData.Model);
        }

        [Fact]
        public void Index_WhenUserIsAdmin_RedirectsToAdminDashboard()
        {
            // Arrange
            _controller.WithIdentity(role: "Admin");

            // Act
            var result = _controller.Index();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Dashboard", redirectToActionResult.ActionName);
            Assert.Equal("Admin", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Index_WhenUserIsAuthenticated_ReturnsViewWithModel()
        {
            // Arrange
            _controller.WithIdentity(role: "User");

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<IncomingDataModel>(viewResult.Model);
            
            Assert.NotNull(model.GoodsDonations);
            Assert.NotNull(model.MoneyDonations);
            Assert.NotNull(model.Disasters);
            
            Assert.Equal(2, model.GoodsDonations.Count);
            Assert.Equal(2, model.MoneyDonations.Count);
            Assert.Equal(2, model.Disasters.Count);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            // Act
            var result = _controller.Privacy();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResultWithErrorViewModel()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileApi.Server.Controllers;
using MobileApi.Server.Core;
using MobileApi.Server.Core.Domain;
using MobileApi.Server.Core.Repositories;
using MobileApi.Server.Infrastructure;
using MobileApi.Server.Models;
using Moq;

namespace MobileApi.Server.Tests
{
    [TestClass]
    public class TestProfileController
    {
        [TestMethod]
        public async Task TestGet_success()
        {
            var profile = new UserProfile()
            {
                FirstName = "FirstName",
                SecondName = "SecondName",
                Patronym = "Patronym",
                Email = "aa@aa.ru",
                AvatarLink = "AvatarLink",
                DateOfBirth = DateTime.MaxValue
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() {Id = Guid.NewGuid().ToString()}));

            userProfileRepository.Setup(x => x.Get(It.IsAny<string>())).Returns(profile);
            
            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var result = await controller.Get() as OkNegotiatedContentResult<ProfileViewModel>;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.FirstName, profile.FirstName);
            Assert.AreEqual(result.Content.SecondName, profile.SecondName);
            Assert.AreEqual(result.Content.Patronym, profile.Patronym);
            Assert.AreEqual(result.Content.Email, profile.Email);
            Assert.AreEqual(result.Content.AvatarLink, profile.AvatarLink);
            Assert.AreEqual(result.Content.DateOfBirth, profile.DateOfBirth);
        }

        [TestMethod]
        public async Task TestGet_not_found()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() { Id = Guid.NewGuid().ToString() }));

            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var result = await controller.Get() as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task TestPost_success()
        {

            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() { Id = Guid.NewGuid().ToString() }));

            userProfileRepository.Setup(x => x.IsExists(It.IsAny<string>())).Returns(false);

            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var viewModel = new ProfileViewModel()
            {
                FirstName = "FirstName1",
                SecondName = "SecondName1",
                Patronym = "Patronym",
                Email = "aa@aa.ru1",
                AvatarLink = "AvatarLink1",
                DateOfBirth = DateTime.MinValue
            };

            var result = await controller.Post(viewModel) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
        }

        [TestMethod]
        public async Task TestPost_conflict()
        {

            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() { Id = Guid.NewGuid().ToString() }));

            userProfileRepository.Setup(x => x.IsExists(It.IsAny<string>())).Returns(true);

            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var viewModel = new ProfileViewModel()
            {
                FirstName = "FirstName1",
                SecondName = "SecondName1",
                Patronym = "Patronym",
                Email = "aa@aa.ru1",
                AvatarLink = "AvatarLink1",
                DateOfBirth = DateTime.MinValue
            };

            var result = await controller.Post(viewModel) as ConflictResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task TestPut_success()
        {
            var profile = new UserProfile()
            {
                FirstName = "FirstName",
                SecondName = "SecondName",
                Patronym = "Patronym",
                Email = "aa@aa.ru",
                AvatarLink = "AvatarLink",
                DateOfBirth = DateTime.MaxValue,
                UserId = Guid.NewGuid().ToString()
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() { Id = Guid.NewGuid().ToString() }));

            userProfileRepository.Setup(x => x.Get(It.IsAny<string>())).Returns(profile);

            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var viewModel = new ProfileViewModel()
            {
                FirstName = "FirstName1",
                SecondName = "SecondName1",
                Patronym = "Patronym",
                Email = "aa@aa.ru1",
                AvatarLink = "AvatarLink1",
                DateOfBirth = DateTime.MinValue
            };

            var result = await controller.Put(viewModel) as OkResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task TestPut_no_content()
        {
            var profile = new UserProfile()
            {
                FirstName = "FirstName",
                SecondName = "SecondName",
                Patronym = "Patronym",
                Email = "aa@aa.ru",
                AvatarLink = "AvatarLink",
                DateOfBirth = DateTime.MaxValue
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() { Id = Guid.NewGuid().ToString() }));

            userProfileRepository.Setup(x => x.Get(It.IsAny<string>())).Returns<UserProfile>(null);

            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var viewModel = new ProfileViewModel()
            {
                FirstName = "FirstName1",
                SecondName = "SecondName1",
                Patronym = "Patronym",
                Email = "aa@aa.ru1",
                AvatarLink = "AvatarLink1",
                DateOfBirth = DateTime.MinValue
            };

            var result = await controller.Put(viewModel) as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task TestDelete_success()
        {
            var profile = new UserProfile()
            {
                FirstName = "FirstName",
                SecondName = "SecondName",
                Patronym = "Patronym",
                Email = "aa@aa.ru",
                AvatarLink = "AvatarLink",
                DateOfBirth = DateTime.MaxValue
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() { Id = Guid.NewGuid().ToString() }));

            userProfileRepository.Setup(x => x.Get(It.IsAny<string>())).Returns(profile);

            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var result = await controller.Delete() as OkResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task TestDelete_no_content()
        {
            var profile = new UserProfile()
            {
                FirstName = "FirstName",
                SecondName = "SecondName",
                Patronym = "Patronym",
                Email = "aa@aa.ru",
                AvatarLink = "AvatarLink",
                DateOfBirth = DateTime.MaxValue
            };

            var unitOfWork = new Mock<IUnitOfWork>();
            var userProfileRepository = new Mock<IUserProfileRepository>();
            var identityRepository = new Mock<IIdentityRepository>();

            identityRepository.Setup(x => x.GetCurrentApplicationUserAsync())
                .Returns(Task.FromResult(new ApplicationUser() { Id = Guid.NewGuid().ToString() }));

            userProfileRepository.Setup(x => x.Get(It.IsAny<string>())).Returns<UserProfile>(null);

            unitOfWork.Setup(x => x.UserProfile).Returns(userProfileRepository.Object);
            unitOfWork.Setup(x => x.Identity).Returns(identityRepository.Object);

            var controller = new ProfileController(unitOfWork.Object);

            var result = await controller.Delete() as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NoContent);
        }
    }
}

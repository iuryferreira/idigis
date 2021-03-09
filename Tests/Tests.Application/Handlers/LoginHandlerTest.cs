using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Application.Handlers;
using Core.Application.Requests;
using Core.Authentication.Contracts;
using Core.Authentication.Services;
using Core.Domain.Entities;
using Core.Persistence.Contracts;
using Core.Shared.Notifications;
using DotNetEnv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Application.Handlers
{
    [TestClass]
    public class LoginHandlerTest
    {
        private readonly IJwtService _jwtService;
        private readonly Mock<IChurchRepository> _repository;

        public LoginHandlerTest ()
        {
            _jwtService = new JwtService();
            _repository = new();
            Env.TraversePath().Load();
        }

        [TestMethod]
        public async Task Should_Return_Null_If_the_Entity_Is_Invalid ()
        {
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("invalid", "");
            Assert.IsNull(await sut.Handle(data, new()));
        }

        [TestMethod]
        public async Task Must_Have_Notification_If_Data_Is_Invalid ()
        {
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("invalid", "");
            var result = await sut.Handle(data, new());
            Assert.IsTrue(sut.Notificator.HasNotifications);
        }

        [TestMethod]
        public async Task Must_Return_Null_If_the_Entity_Does_Not_Exist ()
        {
            _repository.Setup(repository => repository.Get(It.IsAny<Login>())).ReturnsAsync((Church)null);
            _repository.SetupGet(repository => repository.Notifications)
                .Returns(new List<Notification>());
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("not_found@email.com", "any_password");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Must_Have_Notification_If_Church_Not_Found ()
        {
            _repository.Setup(repository => repository.Get(It.IsAny<Login>())).ReturnsAsync((Church)null);
            var notifications = new List<Notification> { new("Repository", "Registro não encontrado.") };
            _repository.SetupGet(repository => repository.Notifications).Returns(notifications);
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("not_found@email.com", "any_password");
            var result = await sut.Handle(data, new());
            Assert.IsTrue(sut.Notificator.HasNotifications);
        }

        [TestMethod]
        public async Task Must_Return_a_Response_Containing_the_User_Data_and_the_Token ()
        {
            var entity = new Church(Guid.NewGuid().ToString(), "Valid Name", new("found@email.com", "valid_password"));
            _repository.Setup(repository => repository.Get(It.IsAny<Login>())).ReturnsAsync(entity);
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("not_found@email.com", "any_password");
            var result = await sut.Handle(data, new());
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.Email);
            Assert.IsNotNull(result.Token);
        }
    }
}

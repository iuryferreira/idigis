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
using Core.Shared.Types;
using DotNetEnv;
using Hash;
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
            Env.TraversePath().Load();
            _jwtService = new JwtService();
            _repository = new();
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
            _repository.Setup(repository => repository.Get(It.IsAny<Property>())).ReturnsAsync((Church)null);
            _repository.SetupGet(repository => repository.Notificator).Returns(new Notificator());
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("not_found@email.com", "any_password");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Must_Return_a_Response_Containing_the_User_Data_and_the_Token ()
        {
            var entity = new Church(Guid.NewGuid().ToString(), "Valid Name", new("found@email.com", new Hashio().Hash("any_password")));
            _repository.Setup(repository => repository.Get(It.IsAny<Property>())).ReturnsAsync(entity);
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("not_found@email.com", "any_password");
            var result = await sut.Handle(data, new());
            Assert.IsNotNull(result.Id);
            Assert.IsNotNull(result.Name);
            Assert.IsNotNull(result.Email);
            Assert.IsNotNull(result.Token);
        }
        
        [TestMethod] 
        public async Task Must_Return_Null_If_the_Password_Is_Not_Correct ()
        {
            var entity = new Church(Guid.NewGuid().ToString(), "Valid Name", new("found@email.com", new Hashio().Hash("any_password")));
            _repository.Setup(repository => repository.Get(It.IsAny<Property>())).ReturnsAsync(entity);
            var sut = new LoginHandler(new Notificator(), _repository.Object, _jwtService);
            var data = new LoginRequest("not_found@email.com", "any");
            var result = await sut.Handle(data, new());
            Assert.IsNull(result);
        }
        
        
    }
}

using System;
using System.Threading.Tasks;
using Core.Application.Handlers.OfferHandlers;
using Core.Application.Requests.OfferRequests;
using Core.Application.Responses.OfferResponses;
using Core.Domain.Entities;
using Core.Persistence.Contracts;
using Core.Shared.Notifications;
using Core.Shared.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Tests.Application.Handlers.OfferHandlers
{
    [TestClass]
    public class CreateOfferHandlerTest
    {
        readonly Mock<IChurchRepository> _repository;

        public CreateOfferHandlerTest ()
        {
            _repository = new();
        }

        [TestMethod]
        public async Task Must_Return_Null_And_Message_if_Request_Is_Invalid ()
        {
            var church = new Church(Guid.NewGuid().ToString(), "valid_name", new("valid_email@email.com", "valid_password"));
            var notificator = new Notificator();

            _repository.Setup(repository => repository.Get(It.IsAny<Property>())).ReturnsAsync(church);
            _repository.SetupGet(repository => repository.Notificator).Returns(notificator);

            var sut = new CreateOfferHandler(_repository.Object, notificator);

            var request = new CreateOfferRequest(Guid.NewGuid().ToString(), 0);
            Assert.IsNull(await sut.Handle(request, new()));
            Assert.IsTrue(notificator.HasNotifications);
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Church_Not_Found ()
        {
            var notificator = new Notificator();

            _repository.Setup(repository => repository.Get(It.IsAny<Property>())).ReturnsAsync((Church)null);
            _repository.SetupGet(repository => repository.Notificator).Returns(notificator);

            var sut = new CreateOfferHandler(_repository.Object, notificator);

            var request = new CreateOfferRequest(Guid.NewGuid().ToString(), 0);
            Assert.IsNull(await sut.Handle(request, new()));
        }

        [TestMethod]
        public async Task Must_Return_Null_if_Church_Is_Not_Updated ()
        {
            var church = new Church(Guid.NewGuid().ToString(), "valid_name", new("valid_email@email.com", "valid_password"));
            var notificator = new Notificator();

            _repository.Setup(repository => repository.Get(It.IsAny<Property>())).ReturnsAsync(church);
            _repository.Setup(repository => repository.Update(It.IsAny<Church>())).ReturnsAsync(false);
            _repository.SetupGet(repository => repository.Notificator).Returns(notificator);

            var sut = new CreateOfferHandler(_repository.Object, notificator);

            var request = new CreateOfferRequest(Guid.NewGuid().ToString(), 3);
            var result = await sut.Handle(request, new());
            Assert.IsNull(await sut.Handle(request, new()));
        }

        [TestMethod]
        public async Task Must_Add_Offer_if_Church_Is_Found_And_Return ()
        {
            var church = new Church(Guid.NewGuid().ToString(), "valid_name", new("valid_email@email.com", "valid_password"));
            var notificator = new Notificator();

            _repository.Setup(repository => repository.Get(It.IsAny<Property>())).ReturnsAsync(church);
            _repository.Setup(repository => repository.Update(It.IsAny<Church>())).ReturnsAsync(true);
            _repository.SetupGet(repository => repository.Notificator).Returns(notificator);

            var sut = new CreateOfferHandler(_repository.Object, notificator);

            var request = new CreateOfferRequest(Guid.NewGuid().ToString(), 3);
            var result = await sut.Handle(request, new());
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreateOfferResponse));
        }
    }
}

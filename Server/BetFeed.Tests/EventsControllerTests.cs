using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BetFeed.Infrastructure.Repository;
using BetFeed.Models;
using BetFeed.Controllers;
using System.Web.Http.Results;
using System.Collections.Generic;
using BetFeed.ViewModels;

namespace BetFeed.Tests
{
    [TestClass]
    public class EventsControllerTests
    {
        [TestInitialize]
        public void AutoMapperConfig()
        {
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void EventMethod_ShouldReturnBadRequestWhenNoIdIsPassed()
        {
            var eventRepositoryMock = new Mock<IRepository<Event>>();

            var eventsController = new EventsController(eventRepositoryMock.Object);

            var result = eventsController.Event(0);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void EventMethod_ShouldReturnNotFoundIfNoEvent()
        {
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            eventRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Event)null);

            var eventsController = new EventsController(eventRepositoryMock.Object);

            var result = eventsController.Event(11);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void EventMethod_ShouldReturnProperEvent()
        {
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            eventRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Event()
                {
                    Id = 1,
                    CategoryId = 1541,
                    CategoryName = "Random",
                    Name = "Super Cup",
                    Matches = new HashSet<BetFeed.Models.Match>() { new BetFeed.Models.Match()
                        {
                            Id = 5,
                            Name = "Best Match",
                            StartDate = DateTime.Now
                        }},
                    UpdatedOn = DateTime.Now,
                    IsLive = false
                });

            var eventsController = new EventsController(eventRepositoryMock.Object);

            var result = eventsController.Event(11);

            var expected = (JsonResult<EventWithMatchesViewModel>)result;


            Assert.AreEqual(1, expected.Content.Id);
            Assert.AreEqual(1, expected.Content.Matches.Count);
            Assert.AreEqual("Super Cup", expected.Content.Name);
        }

        [TestMethod]
        public void NewMatches_ShouldReturnBadRequestWhenNoIdIsPassed()
        {
            var eventRepositoryMock = new Mock<IRepository<Event>>();

            var eventsController = new EventsController(eventRepositoryMock.Object);

            var result = eventsController.NewMatches(0, DateTime.Now);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void NewMatches_ShouldReturnNotFoundIfNoEvent()
        {
            var eventRepositoryMock = new Mock<IRepository<Event>>();
            eventRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Event)null);

            var eventsController = new EventsController(eventRepositoryMock.Object);

            var result = eventsController.NewMatches(11, DateTime.Now);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}

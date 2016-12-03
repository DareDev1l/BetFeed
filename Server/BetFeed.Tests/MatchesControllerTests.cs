using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BetFeed.Infrastructure.Repository;
using BetFeed.Controllers;
using System.Web.Http.Results;
using BetFeed.Models;
using BetFeed.ViewModels;
using System.Collections.Generic;

namespace BetFeed.Tests
{
    [TestClass]
    public class MatchesControllerTests
    {
        [TestInitialize]
        public void AutoMapperConfig()
        {
            AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void MatchMethod_ShouldReturnBadRequestWhenNoIdIsPassed()
        {
            var matchRepositoryMock = new Mock<IRepository<BetFeed.Models.Match>>();

            var matchesController = new MatchesController(matchRepositoryMock.Object);

            var result = matchesController.Match(0);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void MatchMethod_ShouldReturnNotFoundIfNoEvent()
        {
            var matchRepositoryMock = new Mock<IRepository<BetFeed.Models.Match>>();
            matchRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((BetFeed.Models.Match)null);

            var matchesController = new MatchesController(matchRepositoryMock.Object);

            var result = matchesController.Match(5);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void MatchMethod_ShouldReturnProperMatch()
        {
            var matchRepositoryMock = new Mock<IRepository<BetFeed.Models.Match>>();
            matchRepositoryMock.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new BetFeed.Models.Match()
                {
                    Id = 5,
                    Name = "Best-Match",
                    StartDate = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    MatchType = "PreMatch",
                    Bets = new HashSet<Bet>()
                    {
                        new Bet()
                        {
                            Id = 51,
                            Name = "Some Bet",
                            Odds = new HashSet<Odd>()
                            {
                                new Odd()
                                {
                                    Id = 4313,
                                    Name = "1",
                                    SpecialBetValue = "-32.0",
                                    Value = 2.32m,
                                    UpdatedOn = DateTime.Now
                                },
                                new Odd()
                                {
                                    Id = 4314,
                                    Name = "2",
                                    SpecialBetValue = "-32.0",
                                    Value = 2.74m,
                                    UpdatedOn = DateTime.Now
                                }
                            }
                        }
                    }
                });

            var matchesController = new MatchesController(matchRepositoryMock.Object);

            var result = matchesController.Match(14);

            var expected = (JsonResult<MatchWithBetsViewModel>)result;


            Assert.AreEqual(5, expected.Content.Id);
            Assert.AreEqual("Best", expected.Content.First);
            Assert.AreEqual("Match", expected.Content.Second);
        }
    }
}

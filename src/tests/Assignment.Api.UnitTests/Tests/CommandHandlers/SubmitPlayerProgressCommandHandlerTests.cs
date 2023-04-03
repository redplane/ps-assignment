using System.Net;
using Assignment.Apis.Constants;
using Assignment.Apis.Cqrs.CommandHandlers.Players;
using Assignment.Apis.Models.Exceptions;
using Assignment.Apis.Services.Implementations;
using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Models;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Cores.Models;
using Assignment.Cores.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Assignment.Api.UnitTests.Tests.CommandHandlers
{
    [TestFixture]
    public class SubmitPlayerProgressCommandHandlerTests
    {
        #region Properties

        private readonly IServiceCollection _services;

        #endregion

        #region Constructor

        public SubmitPlayerProgressCommandHandlerTests()
        {
            _services = new ServiceCollection();
        }

        #endregion

        #region Life cycle hooks

        [SetUp]
        public void SetUp()
        {
            var quest = new Quest();
            quest.RateFromBet = 5;
            quest.LevelBonusRate = 3;
            quest.TotalPoint = 100;
            quest.Milestones = new[]
            {
                new Milestone
                {
                    TotalPoint = 10,
                    Chips = 1
                },
                new Milestone
                {
                    TotalPoint = 15,
                    Chips = 2
                },
                new Milestone
                {
                    TotalPoint = 30,
                    Chips = 5
                },
                new Milestone
                {
                    TotalPoint = 50,
                    Chips = 7
                },
                new Milestone
                {
                    TotalPoint = 80,
                    Chips = 9
                },
                new Milestone
                {
                    TotalPoint = 100,
                    Chips = 50
                }
            };
            _services.AddScoped<IPlayerService, PlayerService>();
            _services.AddScoped<IQuestService, QuestService>();
            _services.AddSingleton(quest);

             _services.AddScoped(_ =>
            {
                var options = new DbContextOptionsBuilder<AssignmentDbContext>()
                    .UseInMemoryDatabase("AssignmentDbContext")
                    .Options;
                return new AssignmentDbContext(options);
            });
        }

        [TearDown]
        public void TearDown()
        {
            _services.Clear();
        }

        #endregion

        #region Methods

        [Test]
        public void WhenAPlayerIsNotFound_Throws_BusinessException()
        {
            // Arrange
            _services.AddScoped(provider =>
            {
                var playerServiceMock = new Mock<IPlayerService>();
                playerServiceMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync((Player?)null);
                return playerServiceMock.Object;
            });
            _services.AddScoped<SubmitPlayerProgressCommandHandler>();

            // Act
            var serviceProvider = _services.BuildServiceProvider().CreateScope().ServiceProvider;
            var commandHandler = serviceProvider.GetService<SubmitPlayerProgressCommandHandler>();
            
            // Test
            var command = new SubmitPlayerProgressCommand();
            command.PlayerId = Guid.NewGuid();
            command.PlayerLevel = 3;
            command.ChipAmountBet = 10;

            var exception = Assert.CatchAsync<BusinessException>(() => commandHandler!.Handle(command, CancellationToken.None));
            Assert.NotNull(exception);
            Assert.AreEqual(HttpStatusCode.NotFound, exception!.StatusCode);
            Assert.AreEqual(ExceptionCodes.UserNotFound, exception!.MessageCode);
        }

        [Test]
        public async Task WhenNotEnoughPointToCompleteMilestone_Expects_PointWillBeAddedButMilestoneWillBeNot()
        {
            // Arrange
            var playerId = Guid.NewGuid();
            _services.AddScoped<SubmitPlayerProgressCommandHandler>();

            var serviceProvider = _services.BuildServiceProvider().CreateScope().ServiceProvider;
            var quest = serviceProvider.GetService<Quest>();
            var dbContext = serviceProvider.GetService<AssignmentDbContext>()!;
            dbContext.Players!.Add(new Player(playerId) { CurrentMilestone = 0, TotalChips = 0, TotalPoints = 0 });
            await dbContext.SaveChangesAsync();

            // Act
            var commandHandler = serviceProvider.GetService<SubmitPlayerProgressCommandHandler>();

            // Test
            var command = new SubmitPlayerProgressCommand();
            command.PlayerId = playerId;
            command.PlayerLevel = 3;
            command.ChipAmountBet = 10;

            var playerProgress = await commandHandler!.Handle(command, CancellationToken.None);

            Assert.NotNull(playerProgress);
            Assert.AreEqual( 59, playerProgress.TotalQuestPercentCompleted);
            Assert.AreEqual( 59, playerProgress.QuestPointsEarned);
            Assert.NotNull(playerProgress.MilestonesCompleted);
            Assert.AreEqual(3, playerProgress.MilestonesCompleted.Length);

            foreach (var completedMilestone in playerProgress.MilestonesCompleted)
                Assert.AreEqual(quest.Milestones[completedMilestone.MilestoneIndex].Chips, completedMilestone.ChipsAwarded);
        }


        #endregion
    }
}
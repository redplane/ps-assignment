using Assignment.Apis.Models;
using Assignment.Apis.Services;
using Assignment.Apis.Services.Implementations;
using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Models;
using Assignment.Cores.Models;
using Assignment.Cores.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace quest_api_tests
{
    [TestClass]
    public class PlayerServiceTests
    {
        [TestMethod]
        [DataRow(1,1,20,2,2,8)]
        [DataRow(2,2,28,2,2,16)]
        [DataRow(3,5,46,7,3,34)]
        public async Task Process_PassValidData_ShouldReturn200(int playerLevel, int chipAmountBet,
            int totalQuestPercentCompleted, int chipsAwarded, int milestoneIndex, int questPointsEarned)
        {
            // arrange
            var dbContectMock = new Mock<AssignmentDbContext>();
            var player = new Player { Id = Guid.NewGuid(), TotalPoints = 12, CompletedMilestone = 1 };
            dbContectMock.Setup(c => c.FindAsync<Player>(It.IsAny<Guid>()))
                .ReturnsAsync(player);
            var config = new Quest();
            config.RateFromBet = 5;
            config.LevelBonusRate = 3;
            config.TotalPoint = 100;
            config.Milestones = new MileStone[]{
                new() { Chips=0, Points=0},
                 new() { Chips=1, Points=10},
                  new() { Chips=2, Points=20},
                   new() { Chips=5, Points=30},
                   new() { Chips=50, Points=100}
            };
            var data = new SubmitPlayerProgressCommand()
            {
                PlayerId = Guid.NewGuid(),
                PlayerLevel = playerLevel,
                ChipAmountBet = chipAmountBet
            };
            var playerService = new PlayerService(dbContectMock.Object, config);

            // act
            var result = await playerService.SubmitProgressAsync(data, It.IsAny<CancellationToken>());

            // assert
            Assert.AreEqual(totalQuestPercentCompleted, result.TotalQuestPercentCompleted);
            Assert.IsNotNull(result.MilestonesCompleted);
            Assert.AreEqual(chipsAwarded, result.MilestonesCompleted.ChipsAwarded);
            Assert.AreEqual(milestoneIndex, result.MilestonesCompleted.MilestoneIndex);
            Assert.AreEqual(questPointsEarned, result.QuestPointsEarned);
           
        }
    }
}
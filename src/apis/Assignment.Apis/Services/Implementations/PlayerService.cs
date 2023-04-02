using System.Net;
using Assignment.Apis.Constants;
using Assignment.Apis.Models;
using Assignment.Apis.Models.Exceptions;
using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Models;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Cores.Models;
using Assignment.Cores.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Apis.Services.Implementations
{
    public class PlayerService : IPlayerService
    {
        #region Properties

        private readonly AssignmentDbContext _dbContext;

        private readonly Quest _quest;

        #endregion

        #region Constructor

        public PlayerService(AssignmentDbContext dbContext, Quest quest)
        {
            _dbContext = dbContext;
            _quest = quest;
        }

        #endregion

        #region Methods

        public virtual async Task<Player> CreateAsync(CancellationToken cancellationToken)
        {
            var player = new Player(Guid.NewGuid());
            player.TotalPoints = 0;
            player.CurrentMilestone = 0;
            player.TotalChips = 0;

            var insertResult = await _dbContext.Players!.AddAsync(player, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return insertResult.Entity;
        }

        public virtual async Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Players!.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public virtual async Task<Player?> UpdateAsync(Guid id, int totalPoint, int milestone, int totalChip, CancellationToken cancellationToken = default)
        {
            var player = await GetByIdAsync(id, cancellationToken);
            if (player == null)
                throw new BusinessException(HttpStatusCode.NotFound, ExceptionCodes.UserNotFound);

            player.TotalPoints = totalPoint;
            player.CurrentMilestone = milestone;
            player.TotalChips = totalChip;
            _dbContext.Players!.Update(player);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return player;
        }

        public async Task<GetStateResponse> GetStateAsync(Guid playerId, CancellationToken cancellation = default)
        {
            var getStateResult = new GetStateResponse();

            // Get player info.
            var player = await _dbContext.FindAsync<Player>(playerId);
            if (player == null)
                throw new NullReferenceException("Player not found.");

            getStateResult.TotalQuestPercentCompleted = (int)Math.Round((decimal)(player.TotalPoints * 100 / _quest.TotalPoint), MidpointRounding.AwayFromZero);
            getStateResult.LastMilestoneIndexCompleted = player.CurrentMilestone;

            return getStateResult;
        }

        #endregion
    }
}

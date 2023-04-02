using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Models;
using Assignment.Cores.Models.Entities;

namespace Assignment.Businesses.Services.Abstractions
{
    public interface IPlayerService
    {
        #region Methods

        /// <summary>
        /// Create a player asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<Player> CreateAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get player by id asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Player?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a player asynchronously.
        /// </summary>
        Task<Player?> UpdateAsync(Guid id, int totalPoint, int milestone, int totalChip, CancellationToken cancellationToken = default);

        Task<GetStateResponse> GetStateAsync(Guid playerId, CancellationToken cancellation = default);

        #endregion
    }
}

using Assignment.Businesses.Cqrs.Queries.States;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Businesses.ViewModels.Players;
using MediatR;

namespace Assignment.Apis.Cqrs.QueryHandlers.States;

public class GetPlayerStateQueryHandler : IRequestHandler<GetPlayerStateQuery, PlayerStateViewModel>
{
    #region Properties

    private readonly IPlayerService _playerService;

    #endregion

    #region Constructor

    public GetPlayerStateQueryHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    #endregion

    #region Methods

    public virtual async Task<PlayerStateViewModel> Handle(GetPlayerStateQuery query, CancellationToken cancellationToken)
    {
        return await _playerService.GetStateAsync(query.PlayerId, cancellationToken);
    }

    #endregion
}
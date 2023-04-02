using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Cores.Models.Entities;
using MediatR;

namespace Assignment.Apis.Cqrs.CommandHandlers.Players;

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Player>
{
    #region Properties

    private readonly IPlayerService _playerService;

    #endregion

    #region Constructor

    public CreatePlayerCommandHandler(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    #endregion

    #region Methods

    public virtual async Task<Player> Handle(CreatePlayerCommand command, CancellationToken cancellationToken)
    {
        return await _playerService.CreateAsync(cancellationToken);
    }

    #endregion
}
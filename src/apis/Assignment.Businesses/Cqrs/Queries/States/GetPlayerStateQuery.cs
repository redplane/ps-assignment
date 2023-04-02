using Assignment.Businesses.ViewModels.Players;
using MediatR;

namespace Assignment.Businesses.Cqrs.Queries.States;

public class GetPlayerStateQuery : IRequest<PlayerStateViewModel>
{
    #region Properties

    public Guid PlayerId { get; set; }

    #endregion
}
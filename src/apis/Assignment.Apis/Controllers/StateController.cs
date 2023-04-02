using Assignment.Businesses.Cqrs.Queries.States;
using Assignment.Businesses.ViewModels.Players;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Apis.Controllers;

[Route("api/[controller]")]
public class StateController : Controller
{
    #region Properties

    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public StateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    #region Methods

    [HttpGet("{playerId}")]
    public virtual async Task<PlayerStateViewModel> GetAsync([FromRoute] Guid playerId)
    {
        return await _mediator.Send(new GetPlayerStateQuery() {PlayerId = playerId});
    }

    #endregion
}
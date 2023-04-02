using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Cores.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Apis.Controllers;

[Route("api/[controller]")]
public class PlayerController : Controller
{
    #region Properties

    private readonly IMediator _mediator;

    #endregion

    #region Constructor

    public PlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create a player.
    /// </summary>
    /// <returns></returns>
    [HttpPost("")]
    public virtual async Task<Player> CreateAsync()
    {
        return await _mediator.Send(new CreatePlayerCommand());
    }

    #endregion
}
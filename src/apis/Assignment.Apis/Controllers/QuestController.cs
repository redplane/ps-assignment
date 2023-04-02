using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Businesses.ViewModels.Players;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Apis.Controllers
{
    [Route("api/[controller]")]
    public class QuestController : ControllerBase
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public QuestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get progress base on condition.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("progress")]
        public async Task<PlayerProgressViewModel> Progress([FromBody] SubmitPlayerProgressCommand command)
        {
            return await _mediator.Send(command);
        }

        ///// <summary>
        ///// Get the current state of player.
        ///// </summary>
        ///// <param name="playerId"></param>
        ///// <returns></returns>
        //[HttpPost("state/{playerId}")]
        //public async Task<IActionResult> State([FromRoute] Guid playerId)
        //{
        //    var result = await _playerService.GetStateAsync(playerId);
        //    return Ok(result);
        //}

        #endregion
    }
}

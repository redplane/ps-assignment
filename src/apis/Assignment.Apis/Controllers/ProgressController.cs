using Assignment.Businesses.Cqrs.Commands.Players;
using Assignment.Businesses.Services.Abstractions;
using Assignment.Businesses.ViewModels.Players;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Apis.Controllers
{
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {
        #region Properties

        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public ProgressController(IMediator mediator)
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
        [HttpPost]
        public async Task<PlayerProgressViewModel> Progress([FromBody] SubmitPlayerProgressCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}

using Assignment.Businesses.ViewModels.Players;
using MediatR;

namespace Assignment.Businesses.Cqrs.Commands.Players
{
    public class SubmitPlayerProgressCommand : IRequest<PlayerProgressViewModel>
    {
        #region Properties

        public Guid PlayerId { get; set; }

        public int PlayerLevel { get; set; }

        public int ChipAmountBet { get; set; }

        #endregion
    }
}

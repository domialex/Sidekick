using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Platforms;

namespace Sidekick.Application.Game.Stashes
{
    public class ScrollStashUpHandler : ICommandHandler<ScrollStashUpCommand, bool>
    {
        private readonly IKeyboardProvider keyboard;

        public ScrollStashUpHandler(
            IKeyboardProvider keyboard)
        {
            this.keyboard = keyboard;
        }

        public Task<bool> Handle(ScrollStashUpCommand request, CancellationToken cancellationToken)
        {
            keyboard.PressKey("Left");
            return Task.FromResult(true);
        }
    }
}

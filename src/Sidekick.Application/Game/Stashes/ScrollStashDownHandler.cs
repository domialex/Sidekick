using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Platforms;

namespace Sidekick.Application.Game.Stashes
{
    public class ScrollStashDownHandler : ICommandHandler<ScrollStashDownCommand, bool>
    {
        private readonly IKeyboardProvider keyboard;

        public ScrollStashDownHandler(
            IKeyboardProvider keyboard)
        {
            this.keyboard = keyboard;
        }

        public Task<bool> Handle(ScrollStashDownCommand request, CancellationToken cancellationToken)
        {
            keyboard.PressKey("Right");
            return Task.FromResult(true);
        }
    }
}

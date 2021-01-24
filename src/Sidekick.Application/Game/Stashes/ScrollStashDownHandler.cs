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

        public async Task<bool> Handle(ScrollStashDownCommand request, CancellationToken cancellationToken)
        {
            await keyboard.PressKey("Right");
            return true;
        }
    }
}

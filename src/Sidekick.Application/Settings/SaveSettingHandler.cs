using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Settings.Commands;

namespace Sidekick.Application.Settings
{
    public class SaveSettingHandler : ICommandHandler<SaveSettingCommand>
    {
        private readonly SidekickSettings settings;
        private readonly IMediator mediator;

        public SaveSettingHandler(
            SidekickSettings settings,
            IMediator mediator)
        {
            this.settings = settings;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(SaveSettingCommand request, CancellationToken cancellationToken)
        {
            var property = settings.GetType().GetProperty(request.Property);
            var value = property.GetValue(settings);

            if (value != null && request.Value != null && value.Equals(request.Value))
            {
                return Unit.Value;
            }

            property.SetValue(settings, request.Value);
            await mediator.Send(new SaveSettingsCommand(settings));

            return Unit.Value;
        }
    }
}

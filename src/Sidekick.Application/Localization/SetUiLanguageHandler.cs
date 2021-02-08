using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Localization;

namespace Sidekick.Application.Localization
{
    public class SetUiLanguageHandler : ICommandHandler<SetUiLanguageCommand>
    {
        private readonly IMediator mediator;

        public SetUiLanguageHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(SetUiLanguageCommand request, CancellationToken cancellationToken)
        {
            var languages = await mediator.Send(new GetUiLanguagesQuery());
            var name = languages.FirstOrDefault()?.Name;

            if (languages.Any(x => x.Name == request.Name))
            {
                name = request.Name;
            }

            if (!string.IsNullOrEmpty(name))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(request.Name);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(request.Name);
            }

            return Unit.Value;
        }
    }
}

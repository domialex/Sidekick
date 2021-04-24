using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;

namespace Sidekick.Application.Game.Languages
{
    public class GetGameLanguageHandler : IQueryHandler<GetGameLanguageQuery, IGameLanguage>
    {
        private readonly IMediator mediator;

        public GetGameLanguageHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IGameLanguage> Handle(GetGameLanguageQuery request, CancellationToken cancellationToken)
        {
            var languages = await mediator.Send(new GetGameLanguagesQuery());

            var implementationType = languages.FirstOrDefault(x => x.LanguageCode == request.Code)?.ImplementationType;
            if (implementationType != default)
            {
                return (IGameLanguage)Activator.CreateInstance(implementationType);
            }

            return null;
        }
    }
}

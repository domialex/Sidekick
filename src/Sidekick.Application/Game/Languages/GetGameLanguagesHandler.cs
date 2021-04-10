using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Extensions;

namespace Sidekick.Application.Game.Languages
{
    public class GetGameLanguagesHandler : IQueryHandler<GetGameLanguagesQuery, List<GameLanguageAttribute>>
    {
        public Task<List<GameLanguageAttribute>> Handle(GetGameLanguagesQuery request, CancellationToken cancellationToken)
        {
            var result = new List<GameLanguageAttribute>();

            foreach (var type in typeof(GameLanguageAttribute).GetImplementedAttribute())
            {
                var attribute = type.GetAttribute<GameLanguageAttribute>();
                attribute.ImplementationType = type;
                result.Add(attribute);
            }

            return Task.FromResult(result);
        }
    }
}

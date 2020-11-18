using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Business.Apis.PoeNinja
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
  {
    private readonly IPoeNinjaClient poeNinjaClient;
    private readonly IPoeNinjaCache poeNinjaCache;
    private readonly IGameLanguageProvider gameLanguageProvider;

    public DataInitializationStartedHandler(
        IPoeNinjaClient poeNinjaClient,
        IPoeNinjaCache poeNinjaCache,
        IGameLanguageProvider gameLanguageProvider)
    {
      this.poeNinjaClient = poeNinjaClient;
      this.poeNinjaCache = poeNinjaCache;
      this.gameLanguageProvider = gameLanguageProvider;
    }

    public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
    {
      poeNinjaClient.IsSupportingCurrentLanguage = PoeNinjaClient.POE_NINJA_LANGUAGE_CODES.TryGetValue(gameLanguageProvider.Current.LanguageCode, out var languageCode);
      poeNinjaClient.LanguageCode = languageCode;

      await poeNinjaCache.RefreshData();
    }
  }
}

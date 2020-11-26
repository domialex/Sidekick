using MediatR;

namespace Sidekick.Domain.Game.GameLogs.Queries
{
    /// <summary>
    /// Gets the character name of the last whisper message that the player has received inside Path of Exile
    /// </summary>
    public class GetLatestWhisperCharacterNameQuery : IQuery<string>
    {
    }
}

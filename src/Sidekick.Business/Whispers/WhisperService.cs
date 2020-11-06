using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Process;

namespace Sidekick.Business.Whispers
{
    public class WhisperService : IWhisperService
    {
        private readonly INativeProcess pathOfExileProcess;
        private readonly IMediator mediator;

        public WhisperService(
            INativeProcess pathOfExileProcess,
            IMediator mediator)
        {
            this.pathOfExileProcess = pathOfExileProcess;
            this.mediator = mediator;
        }

        public async Task<bool> ReplyToLatestWhisper()
        {
            var characterName = GetLatestWhisperCharacterName();
            if (!string.IsNullOrEmpty(characterName))
            {
                await mediator.Send(new StartWritingChatCommand($"@{characterName} "));
                return true;
            }
            return false;
        }

        private string GetLatestWhisperCharacterName()
        {
            var clientLogFile = pathOfExileProcess?.ClientLogPath;
            if (clientLogFile == null || !pathOfExileProcess.IsPathOfExileInFocus) return string.Empty;

            using (var stream = new FileStream(clientLogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (stream.Length == 0)
                {
                    return null;
                }
                stream.Position = stream.Length - 1;
                while (stream.Position > 0)
                {
                    var currentLine = GetLine(stream);

                    // See if the current line contains a received whisper
                    var match = Regex.Match(currentLine, @"(@From){1}\s.+?(?=:)", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        // No extract only character name
                        var lastWhitespacePos = match.Value.LastIndexOf(" ") + 1;
                        return match.Value[lastWhitespacePos..];
                    }

                    stream.Position -= 1;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the line.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The current line of the stream position</returns>
        private string GetLine(Stream stream)
        {
            // while we have not yet reached start of file, read bytes backwards until '\n' byte is hit
            var lineLength = 0;
            while (stream.Position > 0)
            {
                stream.Position--;
                var byteFromFile = stream.ReadByte();

                if (byteFromFile < 0)
                {
                    // the only way this should happen is if someone truncates the file out from underneath us while we are reading backwards
                    throw new IOException("Error reading from file");
                }
                else if (byteFromFile == '\n')
                {
                    // we found the new line, break out, fs.Position is one after the '\n' char
                    break;
                }

                lineLength++;
                stream.Position--;
            }

            var oldPosition = stream.Position;
            // fs.Position will be right after the '\n' char or position 0 if no '\n' char
            var bytes = new BinaryReader(stream).ReadBytes(lineLength - 1);

            // -1 is the \n
            stream.Position = oldPosition - 1;
            return Encoding.UTF8.GetString(bytes).Replace(System.Environment.NewLine, string.Empty);
        }
    }
}

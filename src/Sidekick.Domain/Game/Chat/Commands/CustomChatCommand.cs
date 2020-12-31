using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Custom chat command
    /// </summary>
    public class CustomChatCommand : ICommand<bool>
    {
        public CustomChatCommand(string command)
        {
            Command = command;
        }

        public string Command { get; }
    }
}

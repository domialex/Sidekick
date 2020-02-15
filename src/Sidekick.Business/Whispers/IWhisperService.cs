using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Business.Whispers
{
    public interface IWhisperService
    {
        Task<bool> ReplyToLatestWhisper();
    }
}

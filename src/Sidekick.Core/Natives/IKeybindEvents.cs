using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Natives
{
    public interface IKeybindEvents
    {
        bool Enabled { get; set; }

        event Func<Task<bool>> OnCloseWindow;
        event Func<Task<bool>> OnExit;
        event Func<Task<bool>> OnPriceCheck;
        event Func<Task<bool>> OnMapInfo;
        event Func<Task<bool>> OnHideout;
        event Func<Task<bool>> OnItemWiki;
        event Func<Task<bool>> OnFindItems;
        event Func<Task<bool>> OnLeaveParty;
        event Func<Task<bool>> OnOpenSearch;
        event Func<Task<bool>> OnTabLeft;
        event Func<Task<bool>> OnTabRight;
        event Func<Task<bool>> OnOpenLeagueOverview;
        event Func<Task<bool>> OnWhisperReply;
    }
}

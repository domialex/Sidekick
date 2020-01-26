using System;
using System.Threading.Tasks;

namespace Sidekick.Platforms
{
    public interface IKeybindEvents
    {
        bool Enabled { get; set; }
        event Func<Task> OnCloseWindow;
        event Func<Task> OnPriceCheck;
        event Func<Task> OnHideout;
        event Func<Task> OnItemWiki;
        event Func<Task> OnFindItems;
        event Func<Task> OnLeaveParty;
        event Func<Task> OnOpenSearch;
        event Func<Task> OnOpenLeagueOverview;
    }
}

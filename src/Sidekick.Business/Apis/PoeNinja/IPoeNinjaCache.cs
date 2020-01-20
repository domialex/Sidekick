using Sidekick.Business.Apis.PoeNinja.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.Business.Apis.PoeNinja
{
    public interface IPoeNinjaCache
    {
        Task Refresh();

        PoeNinjaItem GetItem(Item item);
    }
}

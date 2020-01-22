using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.PoeNinja.Models
{
    public class PoeNinjaQueryResult<T> where T : PoeNinjaResult
    {
        public List<T> Lines { get; set; }
    }
}

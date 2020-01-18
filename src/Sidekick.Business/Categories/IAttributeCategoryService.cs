using Sidekick.Business.Categories.Models;
using System.Collections.Generic;

namespace Sidekick.Business.Categories
{
    public interface IAttributeCategoryService
    {
        List<AttributeCategory> Categories { get; }
    }
}

using System;

namespace Sidekick.Core.DependencyInjection.Configuration
{
  public class SidekickConfigAttribute : Attribute
  {
    public SidekickConfigAttribute(string section)
    {
      Section = section;
    }

    public string Section { get; set; }
  }
}

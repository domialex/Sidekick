using System;

namespace Sidekick.Core.DependencyInjection.Services
{
  public class SidekickServiceAttribute : Attribute
  {
    public SidekickServiceAttribute(Type @interface, LifetimeEnum lifetime = LifetimeEnum.Singleton)
    {
      Interface = @interface;
      Lifetime = lifetime;
    }

    public Type Interface { get; private set; }
    public LifetimeEnum Lifetime { get; private set; }
  }
}

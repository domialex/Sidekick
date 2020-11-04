using AutoFixture;
using Sidekick.TestInfrastructure;

namespace Sidekick.Business.Tests
{
    public abstract class TestContext<T>
    {
        protected TestContext()
        {
            var fixture = new SidekickFixture();
            Subject = fixture.Freeze<T>();
        }

        public T Subject { get; }
    }
}

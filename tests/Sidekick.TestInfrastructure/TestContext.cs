using AutoFixture;
using Moq;
using Sidekick.TestInfrastructure;

namespace Sidekick.Business.Tests
{
    public abstract class TestContext<T>
    {
        public SidekickFixture Fixture { get; set; }

        public T Subject => Fixture.Freeze<T>();

        public Mock<TMock> GetMockFor<TMock>() where TMock : class
        {
            return Fixture.Freeze<Mock<TMock>>();
        }

        public TestContext()
        {
            Fixture = new SidekickFixture();
        }
    }
}

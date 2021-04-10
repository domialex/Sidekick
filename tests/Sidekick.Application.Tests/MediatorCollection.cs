using Xunit;

namespace Sidekick.Application.Tests
{
    [CollectionDefinition(Collections.Mediator)]
    public class MediatorCollection : ICollectionFixture<MediatorFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}

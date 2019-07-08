using FluentAssertions;
using Xunit;

namespace FluentArgs.Test
{
    public class ParameterListTests
    {
        //TODO: pack all parameter configs into a single interface
        //TODO: copy this interface and add:
        //      WithSeparator(...)
        //      IsOptionalWithEmptyDefault()
        [Fact]
        public static void Dummy()
        {
            false.Should().BeTrue();
        }
    }
}

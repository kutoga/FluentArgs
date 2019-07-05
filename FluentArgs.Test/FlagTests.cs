using FluentAssertions;
using Xunit;

namespace FluentArgs.Test
{
    public static class FlagTests
    {
        [Fact]
        public static void GivenAFlag_ItShouldBeRecognized()
        {
            var args = new[] { "-x" };
            var builder = FluentArgsBuilder.New()
                .Flag("-x")
                    .WithDescription("xx") // <-- ok, aber withexamples, withparser und isoptional/required fällt weg
                    ;//.IsDefined()


            //TODO: Flag.IsOptional usw macht keinen Sinn
            true.Should().BeFalse();
        }

        public static void GivenANonPresentFlag_ShouldNotBeRecognized()
        {
        }

        public static void GivenMultipleFlags_ShouldBeHandledCorrect()
        {

        }
    }
}

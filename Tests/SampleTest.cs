using FluentAssertions;
using Xunit;
using Moq;

namespace Tests
{
    public class SampleTest
    {
        [Fact]
        public void DoSomething_Shoud_ReturnHelloWorld_When_CallDoSomethingOfInstance()
        {
            var mock = new Mock<ISampleInterface>();

            mock.Setup(i => i.DoSomething())
                .Returns("Hello World");

            var sample = mock.Object;

            var result = sample.DoSomething();

            result.Should().Be("Hello World");
        }
    }

    public interface ISampleInterface
    {
        string DoSomething();
    }
}
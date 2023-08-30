using FluentAssertions;
using Pippin.Pipes;

namespace Pippin.Tests.Pipes;

public class PipePlugTests
{
    [Test]
    public void Input_And_Process()
    {
        var sut = new ConcretePipePlug();
        
        sut.Input(123);

        sut.HasProcessedInput.Should().BeTrue();
    }
    
    private class ConcretePipePlug : PipePlug<int>
    {
        public bool HasProcessedInput { get; private set; }
        
        protected override void ProcessInput(int input)
        {
            HasProcessedInput = true;
        }
    }
}
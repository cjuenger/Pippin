using FluentAssertions;
using NSubstitute;
using Pippin.Pipes;

namespace Pippin.Tests.Pipes;

public class PipeSocketTests
{
    [Test]
    public void Connect_PipePlug_And_Throw_ArgumentNullException_When_PipePlug_Is_Null()
    {
        var sut = new ConcretePipeSocket<int>();

        var act = () => sut.Connect(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Output_To_Connected_PipePlugs()
    {
        var sut = new ConcretePipeSocket<int>();
        var pipePlug = Substitute.For<IPipePlug<int>>();
        const int input = 123;
        
        sut.Connect(pipePlug);
        sut.Process(input);
        
        pipePlug.Received(1).Input(Arg.Is<int>(i => i == input));
    }
    
    [Test]
    public void Output_And_Throw_ArgumentNullException_When_Output_Is_Null()
    {
        var sut = new ConcretePipeSocket<object>();
        var pipePlug = Substitute.For<IPipePlug<object>>();
        const object input = null!;
        
        sut.Connect(pipePlug);
        var act = () => sut.Process(input!);

        act.Should().Throw<ArgumentNullException>();
    }
    
    private class ConcretePipeSocket<T> : PipeSocket<T>
    {
        public void Process(T input) => Output(input);
    }
}
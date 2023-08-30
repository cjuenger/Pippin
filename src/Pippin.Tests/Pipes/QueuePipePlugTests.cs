using FluentAssertions;
using NSubstitute;
using Pippin.Pipes;
using Pippin.Processors;

namespace Pippin.Tests.Pipes;

public class QueuePipePlugTests
{
    [Test]
    public void Input_And_Throw_ArgumentNullException_When_Input_Is_Null()
    {
        var sut = new ConcreteQueuePipePlug<object>(null!);
        object input = null!;
        
        var act = () => sut.Input(input);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Test]
    public void Input_And_Enqueue_In_Queue_Processor()
    {
        var processor = Substitute.For<IQueueProcessor<int>>();
        var processorFactory = Substitute.For<IProcessorFactory>();
        processorFactory.CreateQueueProcessor(Arg.Any<Action<int>>()).Returns(processor);
        
        var sut = new ConcreteQueuePipePlug<int>(processorFactory);
        const int input = 123;
        
        sut.Input(input);
        
        processor.Received(1).Enqueue(Arg.Is<int>(i => i == input));
    }
    
    private class ConcreteQueuePipePlug<TInput> : QueuePipePlug<TInput>
    {
        public ConcreteQueuePipePlug(IProcessorFactory? processorFactory) : base(processorFactory) {}
        protected override void ProcessInput(TInput input)
        {
            throw new NotImplementedException();
        }
    }
}
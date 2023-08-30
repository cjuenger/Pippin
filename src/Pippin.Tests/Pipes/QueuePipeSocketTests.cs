using FluentAssertions;
using NSubstitute;
using Pippin.Pipes;
using Pippin.Processors;

namespace Pippin.Tests.Pipes;

public class QueuePipeSocketTests
{
    [Test]
    public void Output_And_Throw_ArgumentNullException_When_Output_Is_Null()
    {
        var sut = new ConcreteQueuePipeSocket<object>(null!);
        object output = null!;
        
        var act = () => sut.Process(output);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Test]
    public void Output_And_Enqueue_In_Queue_Processor()
    {
        var processor = Substitute.For<IQueueProcessor<int>>();
        var processorFactory = Substitute.For<IProcessorFactory>();
        processorFactory.CreateQueueProcessor(Arg.Any<Action<int>>()).Returns(processor);
        
        var sut = new ConcreteQueuePipeSocket<int>(processorFactory);
        const int output = 123;
        
        sut.Process(output);
        
        processor.Received(1).Enqueue(Arg.Is<int>(i => i == output));
    }
    
    private class ConcreteQueuePipeSocket<TOutput> : QueuePipeSocket<TOutput>
    {
        public ConcreteQueuePipeSocket(IProcessorFactory? processorFactory) : base(processorFactory) {}
        
        public void Process(TOutput output) => Output(output);
    }
}
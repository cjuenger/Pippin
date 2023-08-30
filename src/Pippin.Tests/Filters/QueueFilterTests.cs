using FluentAssertions;
using NSubstitute;
using Pippin.Filters;
using Pippin.Pipes;
using Pippin.Processors;

namespace Pippin.Tests.Filters;

public class QueueFilterTests
{
    [Test]
    public void Input_And_Enqueue_In_Queue_Processor()
    {
        var (sut, processor) = CreateTestContext<int>();
        const int input = 123;
        
        sut.Input(input);
        
        processor.Received(1).Enqueue(Arg.Is<int>(i => i == input));
    }
    
    [Test]
    public void Input_And_Throw_ArgumentNullException_When_Input_Is_Null()
    {
        var (sut, _) = CreateTestContext<object>();
        object input = null!;
        
        var act = () => sut.Input(input);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Process_And_Output()
    {
        const int input = 123;
        var plug = Substitute.For<IPipePlug<int>>();
        Action<int>? process = default;
        var processorFactory = Substitute.For<IProcessorFactory>();
        processorFactory
            .When(x => x.CreateQueueProcessor(Arg.Any<Action<int>>()))
            .Do(x => process = (Action<int>)x[0]);

        var (sut, _) = CreateTestContext<int>(processorFactory);
        
        sut.Connect(plug);
        process!.Invoke(input);
        
        plug.Received(1).Input(Arg.Is<int>(i => i == input));
    }
    
    [Test]
    public void Dispose()
    {
        var (sut, processor) = CreateTestContext<int>();
        
        sut.Dispose();
        
        processor.Received(1).Dispose();
    }

    private static (ConcreteQueueFilter<T> sut, IQueueProcessor<T> processor) CreateTestContext<T>(IProcessorFactory? processorFactory = null)
    {
        processorFactory ??= Substitute.For<IProcessorFactory>();
        var processor = Substitute.For<IQueueProcessor<T>>();

        processorFactory
            .CreateQueueProcessor(Arg.Any<Action<T>>())
            .Returns(processor);
        
        var sut = new ConcreteQueueFilter<T>(processorFactory);
        
        return (sut, processor);
    } 
    
    private class ConcreteQueueFilter<T> : QueueFilter<T, T>
    {
        public ConcreteQueueFilter(IProcessorFactory? processorFactory) : base(processorFactory) {}
        protected override T Process(T input) => input;
    }
}
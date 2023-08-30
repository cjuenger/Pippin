using FluentAssertions;
using Pippin.Processors;

namespace Pippin.Tests.Processors;

public class QueueProcessorTests
{
    [Test]
    public void Create_And_Throw_ArgumentNullException_When_Argument_Process_Is_Null()
    {
        var act = () => new QueueProcessor<int>(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public async Task Enqueue_And_Process_Item()
    {
        var isActionDone = false;
        // ReSharper disable once ConvertToLocalFunction
        Action<int> action = _ => isActionDone = true;
        const int item = 123;

        var sut = new QueueProcessor<int>(action);
        
        sut.Enqueue(item);

        await WaitUntil(() => isActionDone, 5000);

        isActionDone.Should().BeTrue();
    }
    
    [Test]
    public async Task Enqueue_And_Throw_Exception_Occurred_During_Dequeuing()
    {
        var isActionDone = false;
        // ReSharper disable once ConvertToLocalFunction
        Action<int> action = _ =>
        {
            isActionDone = true;
            throw new Exception();
        };
        const int item = 123;

        var sut = new QueueProcessor<int>(action);
        
        sut.Enqueue(item);

        await WaitUntil(() => isActionDone, 5000);

        var act = () => sut.Enqueue(item);

        act.Should().Throw<Exception>();
    }

     private static async Task WaitUntil(Func<bool> condition, int maxTimeToWait)
     {
         var timeWaited = 0;
         do
         {
             await Task.Delay(200);
             timeWaited += 200;
         } while (!condition.Invoke() && timeWaited < maxTimeToWait);
     }  
}
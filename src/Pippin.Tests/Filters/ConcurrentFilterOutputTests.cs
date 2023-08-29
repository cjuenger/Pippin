using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Pippin.Filters;

namespace Pippin.Tests.Filters;

public class ConcurrentFilterOutputTests
{
    [Test]
    public void Throw_ArgumentNullException_When_FilterInput_To_Chain_Is_Null()
    {
        var sut = new ConcreteConcurrentFilterOutput<int>();
        IFilterInput<int> filterInput = null!;
        
        var act = () => sut.ChainFilter(filterInput);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [TestCase(1)]
    [TestCase(10)]
    public async Task Output_To_Chained_Filters(int countOfFilterInput)
    {
        var sut = new ConcreteConcurrentFilterOutput<int>();

        var listOfFilterInput = Enumerable
            .Range(0, countOfFilterInput)
            .Select(_ => Substitute.For<IFilterInput<int>>())
            .ToList();
        
        const int output = 1987;
        var isLastInputCalled = false;

        foreach (var filterInput in listOfFilterInput)
        {
            filterInput
                .When(x => x.Input(Arg.Any<int>()))
                .Do(_ =>
                {
                    isLastInputCalled = filterInput == listOfFilterInput.Last();
                });
            
            sut.ChainFilter(filterInput);   
        }

        sut.Process(output);

        await WaitUntil(() => isLastInputCalled, 5000);

        foreach (var filterInput in listOfFilterInput)
        {
            filterInput.Received(1).Input(Arg.Is<int>(i => i == output));
        }
    }
    
    [Test]
    public async Task Throw_Exception_That_Occured_While_Dequeuing()
    {
        var sut = new ConcreteConcurrentFilterOutput<object>();
        var filterInput = Substitute.For<IFilterInput<object>>();
        var inputCalled = false;
        
        filterInput
            .When(x => x.Input(Arg.Any<object>()))
            .Do(_ =>
            {
                inputCalled = true;
                throw new Exception();
            });
        
        const int input = 1987;
        
        sut.ChainFilter(filterInput);
        
        sut.Process(input);

        await WaitUntil(() => inputCalled, 5000);

        var act = () => sut.Process(input);
        
        act.Throws<Exception>();
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

    private class ConcreteConcurrentFilterOutput<TOutput> : ConcurrentFilterOutput<TOutput>
    {
        public void Process(TOutput output)
        {
            Output(output);
        }
    }
}
using FluentAssertions;
using NSubstitute;
using Pippin.Filters;

namespace Pippin.Tests.Filters;

public class FilterOutputTests
{
    [Test]
    public void Throw_ArgumentNullException_When_FilterInput_To_Chain_Is_Null()
    {
        var sut = new ConcreteFilterOutput<int>();
        IFilterInput<int> filterInput = null!;
        
        var act = () => sut.ChainFilter(filterInput);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Test]
    public void Throw_ArgumentNullException_When_Output_Is_Null()
    {
        var sut = new ConcreteFilterOutput<object>();
        var filterInput = Substitute.For<IFilterInput<object>>();

        sut.ChainFilter(filterInput);
        
        var act = () => sut.Process(null!);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [TestCase(1)]
    [TestCase(10)]
    public void Output_To_Chained_Filters(int countOfFilterInput)
    {
        var sut = new ConcreteFilterOutput<int>();

        var listOfFilterInput = Enumerable
            .Range(0, countOfFilterInput)
            .Select(_ => Substitute.For<IFilterInput<int>>())
            .ToList();
        
        const int output = 1987;

        foreach (var filterInput in listOfFilterInput)
        {
            sut.ChainFilter(filterInput);   
        }

        sut.Process(output);

        foreach (var filterInput in listOfFilterInput)
        {
            filterInput.Received(1).Input(Arg.Is<int>(i => i == output));
        }
    }

    private class ConcreteFilterOutput<TOutput> : FilterOutput<TOutput>
    {
        public void Process(TOutput output)
        {
            Output(output);
        }
    }
}
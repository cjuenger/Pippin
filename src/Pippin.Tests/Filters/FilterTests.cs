using FluentAssertions;
using NSubstitute;
using Pippin.Filters;
using Pippin.Pipes;

namespace Pippin.Tests.Filters;

public class FilterTests
{
    [Test]
    public void Throw_ArgumentNullException_When_Plug_Is_Null()
    {
        var sut = new ConcreteFilter();
        IPipePlug<object>? plug = null;
        
        var act = () => sut.Connect(plug!);

        act.Should().Throw<ArgumentNullException>();
    }
        
    [Test]
    public void Throw_ArgumentNullException_When_Input_Is_Null()
    {
        var sut = new ConcreteFilter();
        object input = null!;
        
        var act = () => sut.Input(input);

        act.Should().Throw<ArgumentNullException>();
    }
    
    [Test]
    public void Throw_ArgumentNullException_When_Output_Is_Null()
    {
        var sut = new ConcreteFilter(true);
        var input = new object();
        
        var act = () => sut.Input(input);

        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Filter_Input_And_Forward_Output_To_Chained_Filters()
    {
        var sut = new ConcreteFilter();
        var filter = Substitute.For<IFilter<object, object>>();
        const int input = 1987;
        
        sut.Connect(filter);
        
        sut.Input(input);
        
        filter.Received(1).Input(Arg.Is<object>(o => (int)o == input));
    }
    
    private class ConcreteFilter : Filter<object, object>
    {
        private readonly bool _processShouldReturnNull;

        public ConcreteFilter(bool processShouldReturnNull = false)
        {
            _processShouldReturnNull = processShouldReturnNull;
        }
        
        protected override object Process(object input) => _processShouldReturnNull ? null! : input;
    }
}
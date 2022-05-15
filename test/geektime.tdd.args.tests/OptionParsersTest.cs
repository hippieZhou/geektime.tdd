using System.ComponentModel;
using FluentAssertions;

namespace geektime.tdd.args.tests;

public class OptionParsersTest
{
    [Category("Unary")]
    public sealed class UnaryOperationParser
    {
        /// <summary>
        /// Sad Path
        /// </summary>
        [Fact]
        public void should_not_accept_extra_argument_for_single_valued_option()
        {
            var act = () => OptionParsers.Unary(0, Convert.ToInt32)
                .Invoke(new[] {"-p", "8080", "8081"}, new OptionAttribute("p"));
            act.Should()
                .Throw<TooManyArgumentsException>()
                .Where(x => x.Option == "p");
        }

        /// <summary>
        /// Sad Path
        /// </summary>
        /// <param name="arguments"></param>
        [Theory]
        [InlineData("-p -l")]
        [InlineData("-p")]
        public void should_not_accept_insufficient_argument_for_single_valued_option(string arguments)
        {
            var act = () => OptionParsers.Unary(0, Convert.ToInt32)
                .Invoke(arguments.Split(" "), new OptionAttribute("p"));
            act.Should()
                .Throw<InsufficientArgumentsException>()
                .Where(x => x.Option == "p");
        }

        /// <summary>
        /// Default Value
        /// </summary>
        [Fact]
        public void should_set_default_value_to_0_for_int_option()
        {
            OptionParsers.Unary(0, Convert.ToInt32)
                .Invoke(Array.Empty<string>(), new OptionAttribute("p"))
                .Should().Be(0);
        }

        /// <summary>
        /// Happy path
        /// </summary>
        [Fact]
        public void should_parse_value_if_flag_present()
        {
            OptionParsers.Unary(0, Convert.ToInt32)
                .Invoke(new[] {"-p", "8080"}, new OptionAttribute("p"))
                .Should().Be(8080);
        }

        [Fact]
        public void should_raise_exception_if_illegal_value_format()
        {
            var act = ()=> OptionParsers.Unary(new object(), item => throw new IllegalOptionException(item))
                .Invoke(new[] {"-p", "8080"}, new OptionAttribute("p"));
            act.Should()
                .Throw<IllegalOptionException>();
        }
    }
    
    [Category("Bool")]
    public sealed class BoolOperationParser
    {
        /// <summary>
        /// Sad Path
        /// </summary>
        [Fact]
        public void should_not_accept_extra_argument_for_boolean_option()
        {
            var act = () => OptionParsers.Bool().Invoke(new[] {"-l", "t"}, new OptionAttribute("l"));
            act.Should()
                .Throw<TooManyArgumentsException>()
                .Where(x => x.Option == "l");
        }
    
        [Fact(Skip = "default")]
        public void should_not_accept_extra_arguments_for_boolean_option()
        {
            var act = () => OptionParsers.Bool().Invoke(new[] {"-l", "t", "f"}, new OptionAttribute("l"));
            act.Should()
                .Throw<TooManyArgumentsException>()
                .Where(x => x.Option == "l");
        }

        /// <summary>
        /// Default Value
        /// </summary>
        [Fact]
        public void should_set_default_value_to_false_if_option_not_present()
        {
            OptionParsers.Bool().Invoke(Array.Empty<string>(), new OptionAttribute("l"))
                .Should().Be(false);
        }
    
        /// <summary>
        /// Happy path
        /// </summary>
        [Fact]
        public void should_set_value_to_true_if_option_present()
        {
            OptionParsers.Bool().Invoke(new[] {"-l"}, new OptionAttribute("l"))
                .Should().Be(true);
        }
    }
    
    [Category("List")]
    public sealed class ListOperationParser
    {
        [Fact]
        public void should_parse_list_value()
        {
            OptionParsers
                .List(Array.Empty<string>(), Convert.ToString)
                .Invoke(new[] {"-g", "this", "is"}, new OptionAttribute("g"))
                .Should()
                .BeEquivalentTo(new[] {"this", "is"}, options => options.WithStrictOrdering());
        }

        [Fact]
        public void should_not_treat_navigate_int_as_flag()
        {
            OptionParsers
                .List(Array.Empty<int>(), Convert.ToInt32)
                .Invoke(new[] {"-g", "-1", "-2"}, new OptionAttribute("g"))
                .Should()
                .BeEquivalentTo(new[] {-1, -2}, options => options.WithStrictOrdering());
        }

        [Fact]
        public void should_empty_array_as_default_value()
        {
            OptionParsers
                .List(Array.Empty<string>(),Convert.ToString)
                .Invoke(Array.Empty<string>(), new OptionAttribute("g"))
                .Should().BeEmpty();
        }

        [Fact]
        public void should_throw_exception_if_value_parse_cant_parse_value()
        {
            var act = () => OptionParsers
                .List(Array.Empty<string>(), item => throw new IllegalValueException("g", item))
                .Invoke(new[] {"-g", "this", "is"}, new OptionAttribute("g"));
            act.Should()
                .Throw<IllegalValueException>()
                .Where(x => x.Option == "g" && x.Value == "this");
        }
    }
}
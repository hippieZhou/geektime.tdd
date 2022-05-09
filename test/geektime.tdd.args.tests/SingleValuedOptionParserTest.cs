using FluentAssertions;

namespace geektime.tdd.args.tests;

public class SingleValuedOptionParserTest
{
    /// <summary>
    /// Sad Path
    /// </summary>
    [Fact]
    public void should_not_accept_extra_argument_for_single_valued_option()
    {
        var act = () => new SingleValueOptionParser<int>(0, Convert.ToInt32)
            .Parse(new[] {"-p", "8080", "8081"}, new OptionAttribute("p"));
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
        var act = () => new SingleValueOptionParser<int>(0, Convert.ToInt32)
            .Parse(arguments.Split(" "), new OptionAttribute("p"));
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
        new SingleValueOptionParser<int>(0, Convert.ToInt32)
            .Parse(Array.Empty<string>(), new OptionAttribute("p"))
            .Should().Be(0);
    }

    /// <summary>
    /// Happy path
    /// </summary>
    [Fact]
    public void should_parse_value_if_flag_present()
    {
        new SingleValueOptionParser<int>(0, Convert.ToInt32)
            .Parse(new[] {"-p", "8080"}, new OptionAttribute("p"))
            .Should().Be(8080);
    }
}
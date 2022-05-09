using FluentAssertions;

namespace geektime.tdd.args.tests;

public class BooleanOptionParserTest
{
    /// <summary>
    /// Sad Path
    /// </summary>
    [Fact]
    public void should_not_accept_extra_argument_for_boolean_option()
    {
        var act = () => new BooleanOptionParser().Parse(new[] {"-l", "t"}, new OptionAttribute("l"));
        act.Should()
            .Throw<TooManyArgumentsException>()
            .Where(x => x.Option == "l");
    }
    
    [Fact(Skip = "default")]
    public void should_not_accept_extra_arguments_for_boolean_option()
    {
        var act = () => new BooleanOptionParser().Parse(new[] {"-l", "t", "f"}, new OptionAttribute("l"));
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
        new BooleanOptionParser().Parse(Array.Empty<string>(), new OptionAttribute("l"))
            .Should().Be(false);
    }
    
    /// <summary>
    /// Happy path
    /// </summary>
    [Fact]
    public void should_set_value_to_true_if_option_present()
    {
        new BooleanOptionParser().Parse(new[] {"-l"}, new OptionAttribute("l"))
            .Should().Be(true);
    }
}
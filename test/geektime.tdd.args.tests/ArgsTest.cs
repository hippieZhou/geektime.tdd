using FluentAssertions;

namespace geektime.tdd.args.tests;

public class ArgsTest
{
    /// <summary>
    /// -l
    /// </summary>
    [Fact(Skip = "Obsolete")]
    public void should_set_boolean_option_to_true_if_flag_present()
    {
        var option = Args.Parse<BooleanOption>("-l");
        option.Logging.Should().BeTrue();
    }
    
    [Fact(Skip = "Obsolete")]
    public void should_set_boolean_option_to_false_if_not_flag_present()
    {
        var option = Args.Parse<BooleanOption>();
        option.Logging.Should().BeFalse();
    }


    [Fact(Skip = "Obsolete")]
    public void should_parse_int_as_option_value()
    {
        var option = Args.Parse<IntOption>("-p", "8080");
        option.Port.Should().Be(8080);
    }
    
    [Fact(Skip = "Obsolete")]
    public void should_get_string_as_option_value()
    {
        var option = Args.Parse<StringOption>("-d", "/usr/logs");
        option.Directory.Should().Be("/usr/logs");
    }

    /// <summary>
    /// -l -p 8080 -d /usr/logs
    /// </summary>
    [Fact]
    public void should_parse_multi_options()
    {
        var options = Args.Parse<MultiOptions>("-l", "-p", "8080", "-d", "/usr/logs");
        options.Logging.Should().BeTrue();
        options.Port.Should().Be(8080);
        options.Directory.Should().Be("/usr/logs");
    }

    [Fact]
    public void should_throw_illegal_option_exception_if_annotation_not_present()
    {
        var act = () => Args.Parse<OptionsWithoutAnnotation>("-l", "-p", "8080", "-d", "/usr/logs");
        act.Should().Throw<IllegalOptionException>()
        .Where(x => x.Parameter == "Port");
    }

    [Fact]
    public void should_raise_exception_if_type_not_supported()
    {
        var act = () => Args.Parse<OptionsWithUnsupportedtypes>("-l", "abc");
        act.Should().Throw<UnsupportedOptionTypeException>()
            .Where(x => x.Option == "l");
    }

    /// <summary>
    /// -g this is a list -d 1 2 -3 5
    /// </summary>
    [Fact]
    public void Should_Example_2()
    {
        var options = Args.Parse<ListOptions>("-g", "this", "is", "a", "list", "-d", "1", "2", "-3", "5");
        options.Group.Should().BeEquivalentTo("this", "is", "a", "list");
        options.Decimals.Should().BeEquivalentTo(new[] {1, 2, -3, 5});
    }
}
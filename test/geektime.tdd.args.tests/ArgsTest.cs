using FluentAssertions;

namespace geektime.tdd.args.tests;

public class ArgsTest
{
    /// <summary>
    /// -l
    /// </summary>
    [Fact]
    public void should_set_boolean_option_to_true_if_flag_present()
    {
        var option = Args.Parse<BooleanOption>("-l");
        option.Logging.Should().BeTrue();
    }
    
    [Fact]
    public void should_set_boolean_option_to_false_if_not_flag_present()
    {
        var option = Args.Parse<BooleanOption>();
        option.Logging.Should().BeFalse();
    }


    [Fact]
    public void should_parse_int_as_option_value()
    {
        var option = Args.Parse<IntOption>("-p", "8080");
        option.Port.Should().Be(8080);
    }
    
    [Fact]
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
        MultiOptions options = Args.Parse<MultiOptions>("-l", "-p", "8080", "-d", "/usr/logs");
        options.Logging.Should().BeTrue();
        options.Port.Should().Be(8080);
        options.Directory.Should().Be("/usr/logs");
    }

    /// <summary>
    /// -g this is a list -d 1 2 -3 5
    /// </summary>
    [Fact(Skip = "disabled")]
    public void Should_Example_2()
    {
        ListOptions? options = Args.Parse<ListOptions>("-g", "this", "is", "a", "list", "-d", "1", "2", "-3", "5");
        options.Group.Should().BeEquivalentTo(new string[]{"this", "is", "a", "list"});
        options.Decimals.Should().BeEquivalentTo(new int[] {1, 2, 3, 4});
    }
}
using FluentAssertions;

namespace geektime.tdd.args.tests;

public class ArgsMapTest
{
    [Fact]
    public void should_split_without_value()
    {
        var args = Args.ToMap("-b");
        args.Count.Should().Be(1);
        args["b"].Should().BeEquivalentTo(Array.Empty<string>());
    }
    
    [Fact]
    public void should_split_with_value()
    {
        var args = Args.ToMap("-p","8080");
        args.Count.Should().Be(1);
        args["p"].Should().BeEquivalentTo("8080");
    }

    [Fact]
    public void should_split_with_values()
    {
        var args = Args.ToMap("-g", "this", "is", "a", "list");
        args.Count.Should().Be(1);
        args["g"].Should().BeEquivalentTo("this", "is", "a", "list");
    }

    [Fact]
    public void should_split_args_to_map()
    {
        var args = Args.ToMap("-b", "-p", "8080", "-d", "/usr/logs");
        args.Count.Should().Be(3);
        args["b"].Should().BeEquivalentTo(Array.Empty<string>());
        args["p"].Should().BeEquivalentTo(new[] {"8080"});
        args["d"].Should().BeEquivalentTo("/usr/logs");
    }

    [Fact]
    public void should_split_args_list_to_map()
    {
        var args = Args.ToMap("-g", "this", "is", "a", "list", "-d", "1", "2", "-3", "5");
        args.Count.Should().Be(2);
        args["g"].Should().BeEquivalentTo(new[] {"this", "is", "a", "list"}, options => options.WithStrictOrdering());
        args["d"].Should().BeEquivalentTo(new[] {"1", "2", "-3", "5"}, options => options.WithStrictOrdering());
    }
}
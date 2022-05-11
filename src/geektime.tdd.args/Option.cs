namespace geektime.tdd.args;

public record BooleanOption([Option("l")] bool Logging);

public record IntOption([Option("p")] int Port);

public record StringOption([Option("d")] string Directory);

public record MultiOptions(
    [Option("l")] bool Logging,
    [Option("p")] int Port,
    [Option("d")] string Directory);

public record ListOptions(
    [Option("g")] List<string> Group,
    [Option("d")] List<int> Decimals);

public record OptionsWithoutAnnotation(
    [Option("l")] bool Logging,
    int Port,
    [Option("d")] string Directory);

public record OptionsWithUnsupportedtypes(
    [Option("l")] bool Logging);
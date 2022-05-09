namespace geektime.tdd.args;

public record BooleanOption([Option("l")] bool Logging);

public record IntOption([Option("p")] int Port);

public record StringOption([Option("d")] string Directory);

public record MultiOptions(
    [Option("l")] bool Logging,
    [Option("p")] int Port,
    [Option("d")] string Directory);

public record ListOptions(
    [Option("g")] string[] Group,
    [Option("d")] string[] Decimals);
    
    public record OptionsWithoutAnnotation(
        [Option("l")] bool Logging,
        int Port,
        [Option("d")] string Directory);
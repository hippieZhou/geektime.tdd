namespace geektime.tdd.args;

public interface IOptionParser
{
    object Parse(string[] arguments, OptionAttribute option);
}

public class BooleanOptionParser : IOptionParser
{
    public object Parse(string[] arguments, OptionAttribute option)
    {
        var index = arguments.ToList().IndexOf("-" + option.Value);
        if (index + 1 < arguments.Length &&
            !arguments.ElementAt(index + 1).StartsWith("-"))
        {
            throw new TooManyArgumentsException(option.Value);
        }

        return index != -1;
    }
}

public class SingleValueOptionParser<T> : IOptionParser
{
    private readonly Func<string, T> _valueParser;
    private readonly T _defaultValue;

    public SingleValueOptionParser(T defaultValue, Func<string, T> valueParser)
    {
        _defaultValue = defaultValue; 
        _valueParser = valueParser;
    }

    public object Parse(string[] arguments, OptionAttribute option)
    {
        var index = arguments.ToList().IndexOf("-" + option.Value);
        if (index == -1)
        {
            return _defaultValue;
        }
        if (index + 1 == arguments.Length ||
            arguments.ElementAt(index + 1).StartsWith("-"))
        {
            throw new InsufficientArgumentsException(option.Value);
        }

        if (index + 2 < arguments.Length && !arguments.ElementAt(index + 2).StartsWith("-"))
        {
            throw new TooManyArgumentsException(option.Value);
        }

        return _valueParser.Invoke(arguments.ElementAt(index + 1));
    }
}
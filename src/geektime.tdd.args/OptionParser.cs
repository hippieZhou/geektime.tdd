namespace geektime.tdd.args;

public interface IOptionParser
{
    object Parse(string[] arguments, OptionAttribute option);
}

public class BooleanOptionParser : IOptionParser
{
    public object Parse(string[] arguments, OptionAttribute option)
    {
        return arguments.Contains("-" + option.Value);
    }
}

public class SingleValueOptionParser<T> : IOptionParser
{
    private readonly Func<string, T> _valueParser;
    
    public SingleValueOptionParser(Func<string,T> valueParser)
    {
        _valueParser = valueParser;
    }
    public object Parse(string[] arguments, OptionAttribute option)
    {
        var index = arguments.ToList().IndexOf("-" + option.Value);
        var value = arguments.ElementAt(index + 1);
        return _valueParser.Invoke(value);
    }
}
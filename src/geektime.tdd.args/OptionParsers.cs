using System.Collections;
using System.Text.RegularExpressions;

namespace geektime.tdd.args;

public static class OptionParsers
{
    public static Func<string[], OptionAttribute, object> Bool()
    {
        return (arguments, option) =>
            arguments.Any() && GetArgumentList(arguments, option, 0).Any() == false;
    }

    public static Func<string[], OptionAttribute, object> Unary<T>(T defaultValue, Func<string, T> valueParser)
    {
        return (arguments, option) =>
        {
            var argumentList = GetArgumentList(arguments, option, 1);
            return (argumentList.Any() ? valueParser.Invoke(argumentList.First()) : defaultValue) ??
                   throw new InvalidOperationException();
        };
    }

    public static Func<string[], OptionAttribute, IEnumerable<T>> List<T>(IEnumerable<T> defaultValue,
        Func<string, T> valueParser)
    {
        return (arguments, option) =>
        {
            var argumentList = GetArgumentList(arguments, option);
            return arguments.Any() ? argumentList.Select(argument => valueParser.Invoke(argument)).ToList() : defaultValue;
        };
    }

    private static IEnumerable<string> GetArgumentList(string[] arguments, OptionAttribute option)
    {
        var index = Array.IndexOf(arguments, "-" + option.Value);
        return index < 0 ? Array.Empty<string>() : GetValues(arguments, index);
    }

    private static IEnumerable<string> GetArgumentList(string[] arguments, OptionAttribute option, int expectedSize)
    {
        var values = GetArgumentList(arguments, option);
        if (arguments.Any())
        {
            CheckSize(option, expectedSize, values.ToList());
        }

        return values;
    }

    private static void CheckSize(OptionAttribute option, int expectedSize, ICollection<string> values)
    {
        if (values.Count < expectedSize) throw new InsufficientArgumentsException(option.Value);
        if (values.Count > expectedSize) throw new TooManyArgumentsException(option.Value);
    }

    private static IEnumerable<string> GetValues(IReadOnlyCollection<string> arguments, int index)
    {
        var followingFlags = Enumerable.Range(index + 1, arguments.Count - index - 1)
            .Where(x => new Regex("^-[a-zA-Z-]+$").IsMatch(arguments.ElementAt(x)))
            .ToList();
        var followingFlag = followingFlags.Any() ? followingFlags.FirstOrDefault() : arguments.Count;
        return arguments.ToList().GetRange(index + 1, followingFlag - index - 1);
    }
}
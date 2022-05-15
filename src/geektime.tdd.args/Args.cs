using System.Reflection;
using BindingFlags = System.Reflection.BindingFlags;

namespace geektime.tdd.args;

public class Args
{
    private static readonly IDictionary<Type, Func<string[], OptionAttribute, object>> Parsers =
        new Dictionary<Type, Func<string[], OptionAttribute, object>>
        {
            {typeof(bool), OptionParsers.Bool()},
            {typeof(int), OptionParsers.Unary(0, Convert.ToInt32)},
            {typeof(string), OptionParsers.Unary(string.Empty, Convert.ToString)},
            {typeof(List<string>), OptionParsers.List(Array.Empty<string>(), Convert.ToString)},
            {typeof(List<int>), OptionParsers.List(Array.Empty<int>(), Convert.ToInt32)}
        };

    public static T Parse<T>(params string[] args)
    {
        return new Args(Parsers).ParseValue<T>(args);
    }

    private readonly IDictionary<Type, Func<string[], OptionAttribute, object>> _parsers;

    public Args(IDictionary<Type, Func<string[], OptionAttribute, object>> parsers)
    {
        _parsers = parsers;
    }

    private T ParseValue<T>(params string[] args)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();

        var values = constructor?.GetParameters()
            .Select(parameterInfo => ParseOption(args, parameterInfo)).ToArray() ?? Array.Empty<object>();

        return (T) constructor?.Invoke(values)!;
    }

    private object ParseOption(string[] arguments, ParameterInfo parameterInfo)
    {
        var optionAttribute = parameterInfo.GetCustomAttribute<OptionAttribute>();
        if (optionAttribute == null)
        {
            throw new IllegalOptionException(parameterInfo.Name ?? string.Empty);
        }

        try
        {
            return _parsers[parameterInfo.ParameterType].Invoke(arguments, optionAttribute);
        }
        catch (Exception)
        {
            throw new UnsupportedOptionTypeException(optionAttribute.Value);
        }
    }
}
using System.Reflection;
using BindingFlags = System.Reflection.BindingFlags;

namespace geektime.tdd.args;

public static class Args
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
        return Invoke<T>(args, Parsers);
    }

    private static T Invoke<T>(
        string[] args,
        IDictionary<Type, Func<string[], OptionAttribute, object>> parsers)
    {
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public).FirstOrDefault();

        var values = constructor?.GetParameters()
            .Select(parameterInfo => ParseOption(args, parameterInfo, parsers)).ToArray() ?? Array.Empty<object>();

        return (T) constructor?.Invoke(values)!;
    }

    private static object ParseOption(
        string[] arguments,
        ParameterInfo parameterInfo,
        IDictionary<Type, Func<string[], OptionAttribute, object>> parsers)
    {
        var optionAttribute = parameterInfo.GetCustomAttribute<OptionAttribute>();
        if (optionAttribute == null)
        {
            throw new IllegalOptionException(parameterInfo.Name ?? string.Empty);
        }

        try
        {
            return parsers[parameterInfo.ParameterType].Invoke(arguments, optionAttribute);
        }
        catch (Exception)
        {
            throw new UnsupportedOptionTypeException(optionAttribute.Value);
        }
    }
}
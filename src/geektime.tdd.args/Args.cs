using System.Reflection;
using BindingFlags = System.Reflection.BindingFlags;

namespace geektime.tdd.args;

public static class Args
{
    private static readonly IDictionary<Type, Func<string[], OptionAttribute, object>> PARSERS =
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
        var constructor = typeof(T)
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public)[0];

        var values = constructor
            .GetParameters()
            .Select(x => ParseOption(args, x));

        return (T) constructor.Invoke(values.ToArray());
    }

    private static object ParseOption(string[] arguments, ParameterInfo parameter)
    {
        if (parameter.GetCustomAttribute<OptionAttribute>() == null)
        {
            throw new IllegalOptionException(parameter.Name);
        }

        try
        {
            return GetOptionParser(parameter.ParameterType)
                .Invoke(arguments, parameter.GetCustomAttribute<OptionAttribute>());
        }
        catch (Exception)
        {
            var opt = parameter.GetCustomAttribute<OptionAttribute>();
            throw new UnsupportedOptionTypeException(opt.Value);
        }
    }

    private static Func<string[], OptionAttribute, object> GetOptionParser(Type type)
    {
        return PARSERS[type];
    }
}
using System.Reflection;
using BindingFlags = System.Reflection.BindingFlags;

namespace geektime.tdd.args;
public static class Args
{
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
        object value = null;
        var option = parameter.GetCustomAttribute<OptionAttribute>();

        if (parameter.ParameterType == typeof(Boolean))
        {
            value = arguments.Contains("-" + option.Value);
        }

        if (parameter.ParameterType == typeof(Int32))
        {
            var index = arguments.ToList().IndexOf("-" + option.Value);
            value = int.Parse(arguments.ElementAt(index + 1));
        }

        if (parameter.ParameterType == typeof(String))
        {
            var index = arguments.ToList().IndexOf("-" + option.Value);
            value = arguments.ElementAt(index + 1);
        }

        return value;
    }
}
namespace geektime.tdd.di;

public class Context
{
    private readonly Dictionary<Type, Func<object>> _providers = new();

    public void Bind<TInterface>(TInterface instance) where TInterface : class
    {
        _providers[typeof(TInterface)] = () => instance;
    }

    public void Bind<TInterface, TImplementation>()
        where TInterface : class
        where TImplementation : TInterface
    {
        _providers[typeof(TInterface)] = () => CreateInstance(typeof(TImplementation));
    }

    public TInterface Get<TInterface>() where TInterface : class
    {
        if (_providers.ContainsKey(typeof(TInterface)))
        {
            return (TInterface) _providers[typeof(TInterface)].Invoke();
        }

        throw new NotImplementedException(nameof(TInterface));
    }

    private static object CreateInstance(Type concreteType)
    {
        var defaultConstructor = concreteType.GetConstructors()[0];
        var defaultParams = defaultConstructor.GetParameters();
        var parameters = defaultParams.Select(param => CreateInstance(param.ParameterType)).ToArray();
        return defaultConstructor.Invoke(parameters);
    }
}
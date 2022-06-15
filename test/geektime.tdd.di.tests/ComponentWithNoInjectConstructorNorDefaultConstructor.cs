namespace geektime.tdd.di;

public class ComponentWithNoInjectConstructorNorDefaultConstructor : IComponent
{
    public ComponentWithNoInjectConstructorNorDefaultConstructor(string name)
    {
    }
}
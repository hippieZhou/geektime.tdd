namespace geektime.tdd.di;

public class ComponentWithMultiConstructors : IComponent
{
    public ComponentWithMultiConstructors(string name, double value)
    {

    }

    public ComponentWithMultiConstructors(string name)
    {
        
    }
}
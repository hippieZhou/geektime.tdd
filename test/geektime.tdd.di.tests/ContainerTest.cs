using FluentAssertions;

namespace geektime.tdd.di.tests;

public class ContainerTest
{
    public sealed class ComponentConstruction
    {
        private readonly Context _context;

        public ComponentConstruction()
        {
            _context = new Context();
        }

        //TODO:instance
        [Fact]
        public void should_bind_type_to_a_specific_instance()
        {
            var instance = new ComponentWithSpecificInstance();
            _context.Bind<IComponent>(instance);
            _context.Get<IComponent>().Should().BeSameAs(instance);
        }
        
        //TODO:abstract class
        //TODO:interface
        public sealed class ConstructorInjection
        {
            private readonly Context _context1;

            public ConstructorInjection()
            {
                _context1 = new Context();
            }

            //TODO: No args constructor
            [Fact]
            public void should_bind_type_to_a_class_with_default_constructor()
            {
                _context1.Bind<IComponent, ComponentWithDefaultConstructor>();
                var instance = _context1.Get<IComponent>();
                instance.Should().NotBeNull();
                instance.Should().BeOfType<ComponentWithDefaultConstructor>();
            }

            //TODO: with dependencies
            //TODO: A->B->C
        }
        
        public sealed class FieldInjection
        {
            
        }
        
        public sealed class MethodInjection
        {
            
        }
    }
    
    public sealed class DependenciesSelection
    {
        
    }
    
    public sealed class LifecycleManagement
    {
        
    }
}
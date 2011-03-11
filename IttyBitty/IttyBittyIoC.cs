using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoStockPortfolio.Framework
{
    public static class IttyBittyIoC
    {
        [AttributeUsage(AttributeTargets.Constructor)]
        public class InjectionConstructorAttribute : Attribute
        { }

        private enum DependencyType
        {
            None = 0,   // Type is unset
            Delegate,   // A builder function
            Instance,   // A specific instance
            Singleton,  // Dynamically created singleton
            Transient   // Dynamically created transient object
        }

        private class DependencyInfo
        {
            public object Dependency { get; private set; }
            public DependencyType DependencyType { get; private set; }

            public DependencyInfo(DependencyType dependencyType, object dependency)
            {
                DependencyType = dependencyType;
                Dependency = dependency;
            }
        }

        private readonly static IDictionary<Type, DependencyInfo> dependencies = new Dictionary<Type, DependencyInfo>();
        private readonly static IDictionary<Type, object> instances = new Dictionary<Type, object>();

        public static void Register<TContract>(TContract instance)
        {
            dependencies[typeof(TContract)] = new DependencyInfo(DependencyType.Instance, instance);
            instances[typeof(TContract)] = instance;
        }

        public static void Register<TContract, TImplementation>()
        {
            Register<TContract, TImplementation>(false);
        }

        public static void Register<TContract, TImplementation>(bool isSingleton)
        {
            DependencyType dependencyType = isSingleton ? DependencyType.Singleton : DependencyType.Transient;
            dependencies[typeof(TContract)] = new DependencyInfo(dependencyType, typeof(TImplementation));
        }

        public static void Register<TContract>(Func<TContract> builder)
        {
            dependencies[typeof(TContract)] = new DependencyInfo(DependencyType.Delegate, builder);
        }

        public static TContract Resolve<TContract>()
        {
            return (TContract)Resolve(typeof(TContract));
        }

        public static object Resolve(Type contract)
        {
            if (!dependencies.ContainsKey(contract))
                throw new InvalidOperationException(string.Format("Unable to resolve type '{0}'.", contract));
            if (instances.ContainsKey(contract))
                return instances[contract];
            var dependency = dependencies[contract];
            if (dependency.DependencyType == DependencyType.Delegate)
                return ((Delegate)dependency.Dependency).DynamicInvoke();

            var constructorInfo = ((Type)dependency.Dependency).GetConstructors()
                .OrderByDescending(o => (o.GetCustomAttributes(typeof(InjectionConstructorAttribute), false).Count()))
                .ThenByDescending(o => (o.GetParameters().Length))
                .First();
            var parameterInfos = constructorInfo.GetParameters();

            object instance;
            if (parameterInfos.Length == 0)
            {
                instance = Activator.CreateInstance((Type)dependency.Dependency);
            }
            else
            {
                var parameters = new List<object>(parameterInfos.Length);
                foreach (ParameterInfo parameterInfo in parameterInfos)
                {
                    parameters.Add(Resolve(parameterInfo.ParameterType));
                }
                instance = constructorInfo.Invoke(parameters.ToArray());
            }

            if (dependency.DependencyType == DependencyType.Singleton)
            {
                instances[contract] = instance;
            }

            return instance;
        }
    }
}
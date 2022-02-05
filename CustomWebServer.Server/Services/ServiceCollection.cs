namespace CustomWebServer.Server.Services
{
    public class ServiceCollection : IServiceCollection
    {
        private readonly Dictionary<Type, Type> _services;

        public ServiceCollection()
        {
            _services = new Dictionary<Type, Type>();
        }

        public IServiceCollection Add<TService, TImplementation>()
            where TService : class
            where TImplementation : TService
        {
            _services[typeof(TService)] = typeof(TImplementation);

            return this;
        }

        public IServiceCollection Add<TService>() where TService : class
        {
            return Add<TService, TService>();
        }

        public TService Get<TService>()
            where TService : class
        {
            var typeOfService = typeof(TService);

            if (!_services.ContainsKey(typeOfService))
            {
                return null;
            }

            var implementationType = _services[typeOfService];

            return (TService)this.CreateInstance(implementationType);
        }

        public object CreateInstance(Type type)
        {
            if (_services.ContainsKey(type))
            {
                type = _services[type];
            }
            else if (type.IsInterface)
            {
                throw new InvalidOperationException($"Service '{type.FullName}' is not registered.");
            }

            var constructors = type.GetConstructors();

            if (constructors.Length > 1)
            {
                throw new InvalidOperationException("Multiple constructors are not supported in the service resolver.");
            }

            var constructor = constructors.First();

            var parameters = constructor.GetParameters();

            var parameterValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameterType = parameters[i].ParameterType;

                var parameterValue = CreateInstance(parameterType);

                parameterValues[i] = parameterValue;
            }

            return constructor.Invoke(parameterValues);
        }
    }
}

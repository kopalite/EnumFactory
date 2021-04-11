using System;
using System.Collections.Generic;

namespace EnumFactory
{
    public interface IEnumFactory<TEnum, TServiceType> where TEnum : struct, Enum
    {
        TServiceType GetService(TEnum name);
    }

    internal sealed class EnumFactory<TEnum, TServiceType> : IEnumFactory<TEnum, TServiceType> where TEnum : struct, Enum where TServiceType : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<TEnum, Type> _serviceTypes;
        

        public EnumFactory(IServiceProvider serviceProvider, Dictionary<TEnum, Type> serviceTypes)
        {
            _serviceProvider = serviceProvider;
            _serviceTypes = serviceTypes;
        }

        public TServiceType GetService(TEnum serviceName)
        {
            var serviceType = _serviceTypes[serviceName];
            return (dynamic)_serviceProvider.GetService(serviceType);
        }
    }
}

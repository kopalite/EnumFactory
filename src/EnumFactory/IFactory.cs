using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EnumFactory
{
    public interface IFactory<TEnum, TServiceType> where TEnum : struct, Enum
    {
        TServiceType GetService(TEnum name);
    }

    internal sealed class Factory<TEnum, TServiceType> : IFactory<TEnum, TServiceType> where TEnum : struct, Enum where TServiceType : class
    {
        internal static IDictionary<TEnum, Type> ServiceTypes;
        private readonly IServiceProvider _serviceProvider;
        

        public Factory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TServiceType GetService(TEnum serviceName)
        {
            var serviceType = ServiceTypes[serviceName];
            return (TServiceType)_serviceProvider.GetService(serviceType);
        }
    }
}

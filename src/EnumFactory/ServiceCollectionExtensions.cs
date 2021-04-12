using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections.Concurrent;

namespace EnumFactory
{
    public enum Lifecycle
    {
        Scoped,
        Transient,
        Singleton
    }

    public static class ServiceCollectionExtensions
    {
        public static void AddEnumFactory<TEnum, TServiceType>(this IServiceCollection services, Lifecycle lifecycle = Lifecycle.Scoped)  where TEnum : struct, Enum where TServiceType : class
        {
            var serviceTypeSuffix = GetServiceTypeSuffix(typeof(TServiceType).Name);

            var serviceTypes = GetDerivedTypes(typeof(TServiceType));

            var serviceTypesMap = GetServiceTypesMap<TEnum>(serviceTypes, serviceTypeSuffix);

            foreach (var serviceType in serviceTypesMap.Values)
            {
                _ = lifecycle switch
                {
                    Lifecycle.Scoped => services.AddScoped(serviceType),
                    Lifecycle.Transient => services.AddTransient(serviceType),
                    Lifecycle.Singleton => services.AddSingleton(serviceType),
                    _ => throw new ArgumentOutOfRangeException(nameof(lifecycle))
                };
            }

            EnumFactory<TEnum, TServiceType>.ServiceTypes = new ConcurrentDictionary<TEnum, Type>(serviceTypesMap);
            static IEnumFactory<TEnum, TServiceType> factory(IServiceProvider sp) => new EnumFactory<TEnum, TServiceType>(sp);
            
            _ = lifecycle switch
            {
                Lifecycle.Scoped => services.AddScoped(factory),
                Lifecycle.Transient => services.AddTransient(factory),
                Lifecycle.Singleton => services.AddSingleton(factory),
                _ => throw new ArgumentOutOfRangeException(nameof(lifecycle))
            };
        }  

        private static string GetServiceTypeSuffix(string serviceTypeName)
        {
            var words = Regex.Replace(serviceTypeName, "(?!^)([A-Z])", " $1") ?? string.Empty;
            var suffix = words.Split(' ', StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

            if (string.IsNullOrWhiteSpace(suffix))
            {
                throw new Exception($"Could not determine suffix for factory named instances basing on {serviceTypeName}. It should be pascal cased type name (e.g. IOrderService, so Service will be taken as suffix).");
            }

            return suffix;
        }

        private static Type[] GetDerivedTypes(Type parentType)
        {
            var deriviedTypes = AppDomain.CurrentDomain.GetAssemblies()
                                                       .SelectMany(s => s.GetTypes())
                                                       .Where(p => p != parentType && parentType.IsAssignableFrom(p))
                                                       .ToArray();
            return deriviedTypes;
        }

        private static IDictionary<TEnum, Type> GetServiceTypesMap<TEnum>(Type[] serviceTypes, string suffix) where TEnum : struct, Enum
        {
            var serviceTypesMap = Enum.GetValues<TEnum>().ToDictionary(x => x, x => serviceTypes.FirstOrDefault(st => st.Name == x + suffix));
            var missingTypes = string.Join(',', serviceTypesMap.Where(x => x.Value == null).Select(x => x.Key));

            if (!string.IsNullOrWhiteSpace(missingTypes))
            {
                throw new Exception($"Could not find the implementation types for following {typeof(TEnum).Name} values: {missingTypes}. Factory demands 1-1 mapping between enum values and convention-based named types: (EnumValue){suffix}.");
            }

            if (serviceTypes.Length > serviceTypesMap.Count)
            {
                throw new Exception($"There are more implementations than required by {typeof(TEnum).Name} values: Factory demands 1-1 mapping between enum values and convention-based named types: (EnumValue){suffix}.");
            }

            return serviceTypesMap;
        }
    }


}


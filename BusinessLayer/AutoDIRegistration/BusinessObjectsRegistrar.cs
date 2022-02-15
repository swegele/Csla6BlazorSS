using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BusinessLayer.AutoDIRegistration
{
    internal class BusinessObjectsRegistrar
    {
        private List<Type> _businessObjectFactories = new List<Type>();

        internal BusinessObjectsRegistrar(Type scanInput)
        {
            var scanner = new BusinessObjectsFactoryScanner(scanInput);
            var discoveredFactories = scanner.DiscoverBusinessObjectFactories();
            _businessObjectFactories.AddRange(discoveredFactories);
        }

        internal IServiceCollection AddModulesToDi(IServiceCollection services)
        {
            if (_businessObjectFactories is null) throw new NullReferenceException("Business object discovery failed.");

            AddModuleServices(services);

            return services;
        }

        private void AddModuleServices(IServiceCollection services)
        {
            foreach (var type in _businessObjectFactories!)
            {
                services.AddScoped(type);
            }
        }
    }
}

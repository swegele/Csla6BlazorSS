using BusinessLayer.AutoDIRegistration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessLayer.ExtensionMethods
{
    /// <summary>
    /// Extension methods to add required SmartModuleServices
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers business object factory services to the DI container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type">Pick any one type from an assembly where business object factories will be found</param>
        /// <returns></returns>
        public static IServiceCollection AutoRegisterBusinessObjectFactories(this IServiceCollection services, Type type)
        {
            try
            {
                var registrar = new BusinessObjectsRegistrar(type);
                registrar.AddModulesToDi(services);
                services.AddSingleton(typeof(BusinessObjectsRegistrar), (provider) => { return registrar; });
                return services;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

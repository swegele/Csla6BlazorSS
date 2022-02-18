using Microsoft.Extensions.DependencyInjection;

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
        public static IServiceCollection AutoRegisterBusinessObjectFactories(this IServiceCollection services)
        {
            services.DiscoverTypes()
                .Where((type) =>
                {
                    //TypeDiscoveryOptionsBuilder already checks for !IsAbstract so we don't need that here
                    if (type.IsClass &&
                        typeof(IBusinessObjectFactory).IsAssignableFrom(type))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                })
                .AsThemselves()
                .AsTransient()
                .Register();


            return services;
        }
    }
}

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
            services.DiscoverTypesIn(typeof(AlphaInfo).Assembly)
                .Where((type) =>
                {
                    if (type.IsClass &&
                        !type.IsAbstract &&
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

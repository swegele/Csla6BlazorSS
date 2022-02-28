
using Csla;

namespace BusinessLayer.ExtensionMethods
{
    /// <summary>
    /// Extension methods to add required SmartModuleServices
    /// </summary>
    public static class ApplicationContextExtensions
    {
        /// <summary>
        /// Convenience extension method on <see cref="ApplicationContext"/> which calls <see cref="ApplicationContext.CreateInstanceDI(System.Type, object[])"/>
        /// </summary>
        /// <param name="appContext"></param>
        /// <returns></returns>
        public static T CreateInstanceViaDI<T>(this ApplicationContext appContext)
        {
            return (T)appContext.CreateInstanceDI(typeof(T));
        }
    }
}

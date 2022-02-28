using Csla;
using Csla.DataPortalClient;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.ExtensionMethods;

namespace BusinessLayer
{
    [Serializable()]
    public abstract class DemoReadOnlyListBase<T, C> : ReadOnlyListBase<T, C>
        where T : DemoReadOnlyListBase<T, C>, new()
        where C : DemoReadOnlyBase<C>, new()
    {
        protected static IDataPortal<T> GetDataPortal(ApplicationContext appContext)
        {
            var portalFactory = appContext.CreateInstanceViaDI<DataPortalFactory>();
            var portal = portalFactory.GetPortal<T>();
            return portal;
        }

        protected static T CslaSafeConstructor(ApplicationContext appContext)
        {
            var portal = GetDataPortal(appContext);
            return portal.Fetch(new BaseOnlyEmptySemaphoreObject());
        }

        protected DemoDataAdapterManager GetDataManager()
        {
            return ApplicationContext.CreateInstanceViaDI<DemoDataAdapterManagerFactory>().GetManager();
        }

        /// <summary>
        /// Corresponds to factory CslaSafeConstructor above
        /// </summary>
        /// <param name="semaphoreObject"></param>
        [RunLocal]
        [Fetch]
        private void DP_Fetch(BaseOnlyEmptySemaphoreObject semaphoreObject)
        {
        }
    }
}

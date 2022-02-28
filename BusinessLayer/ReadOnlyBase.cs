using Csla;
using Csla.DataPortalClient;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.ExtensionMethods;

namespace BusinessLayer
{
    [Serializable()]
    public abstract class DemoReadOnlyBase<T> : ReadOnlyBase<T> 
        where T : ReadOnlyBase<T>, new()
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
        /// Corresponds with factory CslaSafeConstructor above
        /// </summary>
        /// <param name="emptySemaphore"></param>
        [RunLocal]
        [Fetch]
        private void DP_Fetch(BaseOnlyEmptySemaphoreObject emptySemaphore)
        {
        }

    }
}

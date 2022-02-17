using Csla;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer
{
    public abstract class DemoReadOnlyBaseFactory<T> : IBusinessObjectFactory
        where T : DemoReadOnlyBase<T>, new()
    {
        public readonly IDataPortal<T> Portal;

        public readonly ApplicationContext ApplicationContext;

        protected DemoReadOnlyBaseFactory(IDataPortalFactory portalFactory, ApplicationContext applicationContext)
        {
            Portal = portalFactory.GetPortal<T>();
            ApplicationContext = applicationContext;
        }

        /// <summary>
        /// Csla friendly way to create a new object that should replace any direct calls to CTOR in derived objects.  
        /// Updates to Csla5 added code analyzer checks that no objects have any 
        /// calls directly to constructors.  This forces objects to go through the DataPortal/ObjectFactory methods which 
        /// sets up all the various object states properly (isdirty isnew etc etc)
        /// </summary>
        /// <returns>an empty object of this type</returns>
        public T CslaSafeConstructor()
        {
            return Portal.Fetch(new BaseOnlyEmptySemaphoreObject());
        }
    }

    [Serializable()]
    public abstract class DemoReadOnlyBase<T> : ReadOnlyBase<T> 
        where T : ReadOnlyBase<T>, new()
    {
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

using Csla;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer
{
    public abstract class DemoReadOnlyListBaseFactory<T, C>
        where T : DemoReadOnlyListBase<T, C>, new()
        where C : DemoReadOnlyBase<C>, new()
    {
        protected IDataPortal<T> Portal { get; set; }

        protected ApplicationContext ApplicationContext { get; set; }

        /// <summary>
        /// Csla friendly way to create a new object.  Updates to Csla5 added code analyzer checks that no objects have any 
        /// calls directly to constructors.  Forces objects to go through the DataPortal/ObjectFactory and properly manages object state
        /// </summary>
        /// <returns>an empty object of this type</returns>
        public T CslaSafeConstructor()
        {
            return Portal.Fetch(new BaseOnlyEmptySemaphoreObject());
        }
    }

    [Serializable()]
    public abstract class DemoReadOnlyListBase<T, C> : ReadOnlyListBase<T, C>
        where T : DemoReadOnlyListBase<T, C>, new()
        where C : DemoReadOnlyBase<C>, new()
    {
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

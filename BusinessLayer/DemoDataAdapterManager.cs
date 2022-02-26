using Csla;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer
{
    public class DemoDataAdapterManagerFactory
    {
        ApplicationContext _ApplicationContext { get; set; }

        public DemoDataAdapterManagerFactory(ApplicationContext applicationContext)
        {
            _ApplicationContext = applicationContext;
        }

        public DemoDataAdapterManager GetManager()
        {
            return DemoDataAdapterManager.GetExistingOrCreateNew(_ApplicationContext);
        }
    }

    /// <summary>
    /// Faked - Provides an automated way to reuse open
    /// database connections within the context
    /// of a single data portal operation.
    /// </summary>
    /// <remarks>
    /// This type stores the open adapter
    /// in <see cref="Csla.ApplicationContext.LocalContext" />
    /// and uses reference counting through
    /// <see cref="IDisposable" /> to keep the adapter connection
    /// open for reuse by child objects, and to automatically
    /// dispose the connection when the last consumer
    /// has called Dispose."
    /// </remarks>
    public class DemoDataAdapterManager : IDisposable
    {
        private object _dbAdapter;
        public const string CONST_MYKEY_FOR_APPLICATIONCONTEXT = "__db:Main";
        private ApplicationContext ApplicationContext;
        public readonly Guid _guid = Guid.NewGuid();  //testing - allows tracking of new or existing jtdataadaptermanagers

        internal DemoDataAdapterManager(ApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;

            // open connection if we have crossed the logical and/or physical dataportal so it is safe to access the database wether this code
            //  is deployed in 2 or n tier mode
            if (ApplicationContext.LogicalExecutionLocation == ApplicationContext.LogicalExecutionLocations.Server)
            {
                _dbAdapter = new object();
            }
            else
            {
                //you tried to hit the database without going through the dataportal.  If this code is deployed in a 3tier scenario it would blow up because
                //  the client cannot access the database directly.  This error helps us to code properly since we don't likely develop in 3 tier mode
                throw new Exception("The Database can only be accessed on the server side of DataPortal");
            }
        }

        internal static DemoDataAdapterManager GetExistingOrCreateNew(ApplicationContext applicationContext)
        {
            DemoDataAdapterManager mgr;

            //lock scoped applicationcontext (which should effect user's own instance rather than static effecting all users)
            lock (applicationContext.LocalContext)
            {
                if (applicationContext.LocalContext.Contains(CONST_MYKEY_FOR_APPLICATIONCONTEXT))
                {
                    mgr = (DemoDataAdapterManager)(applicationContext.LocalContext[CONST_MYKEY_FOR_APPLICATIONCONTEXT]);
                }
                else
                {
                    mgr = new DemoDataAdapterManager(applicationContext);
                    applicationContext.LocalContext[CONST_MYKEY_FOR_APPLICATIONCONTEXT] = mgr;
                }
            }

            mgr.AddReference();
            return mgr;
        }

        #region  Reference counting

        private int _refCount;

        /// <summary>
        /// Gets the current reference count for this
        /// object.
        /// </summary>
        internal int ReferenceCount
        {
            get { return _refCount; }
        }

        internal void AddReference()
        {
            _refCount += 1;
        }

        private void HandleCheckForLastReference()
        {
            //lock scoped applicationcontext (which should effect user's own instance rather than static effecting all users)
            lock (ApplicationContext.LocalContext)
            {
                _refCount -= 1;
                if (_refCount == 0)
                {
                    if (_dbAdapter != null)
                    {
                        //would dispose here were this a real DAL db connection
                        //_dbAdapter.Dispose();
                    }

                    ApplicationContext.LocalContext.Remove(CONST_MYKEY_FOR_APPLICATIONCONTEXT);
                }
            }
        }

        #endregion

        #region  IDisposable

        /// <summary>
        /// Dispose object.  When last reference to is disposed 
        /// then the underlying DAL db connection is disposed
        /// </summary>
        public void Dispose()
        {
            HandleCheckForLastReference();
        }

        #endregion

    }
}

using Csla;
using System;

namespace BusinessLayer
{
    public class AlphaInfoListFactory : DemoReadOnlyListBaseFactory<AlphaInfoList, AlphaInfo>
    {
        public AlphaInfoListFactory(DataPortal<AlphaInfoList> portal, ApplicationContext applicationContext)
        {
            Portal = portal;
            ApplicationContext = applicationContext;
        }

        public AlphaInfoList GetAll()
        {
            return Portal.Fetch();
        }

        internal AlphaInfoList GetEmpty()
        {
            return CslaSafeConstructor();
        }
    }

    [Serializable]
    public class AlphaInfoList : DemoReadOnlyListBase<AlphaInfoList, AlphaInfo>
    {
        public AlphaInfoList()
        {

        }

        [Fetch]
        private void Fetch(
            [Inject] AlphaInfoFactory alphaFactory,
            [Inject] DemoDataAdapterManagerFactory cxnManagerFactory)
        {
            using (var cxnManager = cxnManagerFactory.GetManager())
            {
                RaiseListChangedEvents = false;
                IsReadOnly = false;

                //**NOTE here the GUID of the cxnManager - should be same as previous if this is a chained call

                // would normally be loading values from DAL
                for (int i = 0; i < 5; i++)
                {
                    Add(AlphaInfo.Load(alphaFactory, Guid.NewGuid()));
                }

                IsReadOnly = true;
                RaiseListChangedEvents = true;
            }
        }
    }
}

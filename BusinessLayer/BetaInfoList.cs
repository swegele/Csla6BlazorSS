using Csla;
using System;

namespace BusinessLayer
{
    public class BetaInfoListFactory : DemoReadOnlyListBaseFactory<BetaInfoList, BetaInfo>
    {
        public BetaInfoListFactory(DataPortal<BetaInfoList> portal, ApplicationContext applicationContext)
        {
            Portal = portal;
            ApplicationContext = applicationContext;
        }

        public BetaInfoList GetAll()
        {
            return Portal.Fetch();
        }

        internal BetaInfoList GetEmpty()
        {
            return CslaSafeConstructor();
        }
    }

    [Serializable]
    public class BetaInfoList : DemoReadOnlyListBase<BetaInfoList, BetaInfo>
    {
        public BetaInfoList()
        {

        }

        [Fetch]
        private void Fetch(
            [Inject] AlphaInfoListFactory alphaInfoListFactory,
            [Inject] BetaInfoFactory betaFactory,
            [Inject] DemoDataAdapterManagerFactory cxnManagerFactory)
        {
            using (var cxnManager = cxnManagerFactory.GetManager())
            {
                RaiseListChangedEvents = false;
                IsReadOnly = false;

                //**NOTE here the GUID of the cxnManager - should be same as previous if this is a chained call

                //example of fetching another list for some business reason which should re-use the dataadaptermanager and applicationcontext
                var alphaList = alphaInfoListFactory.GetAll();

                // would normally be loading values from DAL
                for (int i = 0; i < 5; i++)
                {
                    Add(BetaInfo.Load(betaFactory, Guid.NewGuid()));
                }

                IsReadOnly = true;
                RaiseListChangedEvents = true;
            }
        }
    }
}

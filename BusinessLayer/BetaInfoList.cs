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

        public BetaInfoList GetByAll()
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
                // would normally be loading values from DAL

                //example of fetching another list for some business reason which should re-use the dataadaptermanager and applicationcontext
                var alphaList = alphaInfoListFactory.GetByAll();

                for (int i = 0; i < 5; i++)
                {
                    Add(BetaInfo.Load(betaFactory, Guid.NewGuid()));
                }
            }
        }
    }
}

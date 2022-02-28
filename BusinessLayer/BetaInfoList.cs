using Csla;
using System;
using BusinessLayer.ExtensionMethods;

namespace BusinessLayer
{
    [Serializable]
    public class BetaInfoList : DemoReadOnlyListBase<BetaInfoList, BetaInfo>
    {
        public BetaInfoList()
        {

        }

        public static BetaInfoList GetAll(ApplicationContext appContext)
        {
            return GetDataPortal(appContext).Fetch();
        }

        [Fetch]
        private void Fetch()
        {
            using (var cxnManager = GetDataManager())
            {
                RaiseListChangedEvents = false;
                IsReadOnly = false;

                //**NOTE here the GUID of the cxnManager - should be same as previous if this is a chained call

                //example of fetching another list for some business reason which should re-use the dataadaptermanager and applicationcontext
                var alphaList = AlphaInfoList.GetAll(ApplicationContext);

                // would normally be loading values from DAL
                for (int i = 0; i < 5; i++)
                {
                    Add(BetaInfo.Load(ApplicationContext, Guid.NewGuid()));
                }

                IsReadOnly = true;
                RaiseListChangedEvents = true;
            }
        }
    }
}

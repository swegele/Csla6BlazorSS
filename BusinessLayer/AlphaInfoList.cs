using Csla;
using System;
using BusinessLayer.ExtensionMethods;

namespace BusinessLayer
{
    [Serializable]
    public class AlphaInfoList : DemoReadOnlyListBase<AlphaInfoList, AlphaInfo>
    {
        public AlphaInfoList()
        {

        }

        public static AlphaInfoList GetAll(ApplicationContext appContext)
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

                // would normally be loading values from DAL
                for (int i = 0; i < 5; i++)
                {
                    Add(AlphaInfo.Load(ApplicationContext, Guid.NewGuid()));
                }

                IsReadOnly = true;
                RaiseListChangedEvents = true;
            }
        }
    }
}

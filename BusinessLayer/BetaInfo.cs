using Csla;
using System;
using BusinessLayer.ExtensionMethods;

namespace BusinessLayer
{

    [Serializable]
    public class BetaInfo : DemoReadOnlyBase<BetaInfo>
    {
        public BetaInfo()
        {

        }

        public static BetaInfo GetById(ApplicationContext appContext, Guid id)
        {
            return GetDataPortal(appContext).Fetch(id);
        }

        public static readonly PropertyInfo<Guid> IdPropertyInfo = RegisterProperty<Guid>(c => c.Id);
        public Guid Id
        {
            get
            {
                return GetProperty(IdPropertyInfo);
            }
        }

        public static readonly PropertyInfo<string> NamePropertyInfo = RegisterProperty<string>(c => c.Name);
        public string Name
        {
            get
            {
                return GetProperty(NamePropertyInfo).Trim();
            }
        }

        [Fetch]
        private void Fetch(Guid id)
        {
            using (var cxnManager = GetDataManager())
            {
                // would normally be loading values from DAL

                LoadProperty(IdPropertyInfo, id);
                LoadProperty(NamePropertyInfo, this.Id.ToString());
            }
        }


        public static BetaInfo Load(ApplicationContext appContext, Guid id)
        {
            var beta = CslaSafeConstructor(appContext);
            beta.LoadProperty(IdPropertyInfo, id);
            beta.LoadProperty(NamePropertyInfo, beta.Id.ToString());

            return beta;
        }
    }
}

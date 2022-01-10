using Csla;
using System;

namespace BusinessLayer
{
    public class AlphaInfoFactory : DemoReadOnlyBaseFactory<AlphaInfo>
    {
        public AlphaInfoFactory(DataPortal<AlphaInfo> portal, ApplicationContext applicationContext)
        {
            Portal = portal;
            ApplicationContext = applicationContext;
        }

        public AlphaInfo GetById(Guid id)
        {
            return Portal.Fetch(id);
        }
    }

    [Serializable]
    public class AlphaInfo : DemoReadOnlyBase<AlphaInfo>
    {
        public AlphaInfo()
        {

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
        private void Fetch(
            Guid id,
            [Inject] DemoDataAdapterManagerFactory cxnManagerFactory)
        {
            using (var cxnManager = cxnManagerFactory.GetManager())
            {
                // would normally be loading values from DAL

                LoadProperty(IdPropertyInfo, id);
                LoadProperty(NamePropertyInfo, this.Id.ToString());
            }
        }


        public static AlphaInfo Load(AlphaInfoFactory alphaFactory, Guid id)
        {
            var alpha = alphaFactory.CslaSafeConstructor();
            alpha.LoadProperty(IdPropertyInfo, id);
            alpha.LoadProperty(NamePropertyInfo, alpha.Id.ToString());

            return alpha;
        }
    }
}

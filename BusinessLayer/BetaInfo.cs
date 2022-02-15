using Csla;
using System;

namespace BusinessLayer
{
    public class BetaInfoFactory : DemoReadOnlyBaseFactory<BetaInfo>
    {
        [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
        public BetaInfoFactory(IDataPortalFactory portal, ApplicationContext applicationContext)
        {
            Portal = portal.GetPortal<BetaInfo>();
            ApplicationContext = applicationContext;
        }

        public BetaInfoFactory()
        {

        }

        public BetaInfo GetById(Guid id)
        {
            return Portal.Fetch(id);
        }
    }

    [Serializable]
    public class BetaInfo : DemoReadOnlyBase<BetaInfo>
    {
        public BetaInfo()
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


        public static BetaInfo Load(BetaInfoFactory BetaFactory, Guid id)
        {
            var beta = BetaFactory.CslaSafeConstructor();
            beta.LoadProperty(IdPropertyInfo, id);
            beta.LoadProperty(NamePropertyInfo, beta.Id.ToString());

            return beta;
        }
    }
}

using Csla;
using System;

namespace BusinessLayer
{
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
        private void Fetch(Guid id)
        {
            using (var cxnManager = GetDataManager())
            {
                // would normally be loading values from DAL

                LoadProperty(IdPropertyInfo, id);
                LoadProperty(NamePropertyInfo, this.Id.ToString());
            }
        }


        public static AlphaInfo Load(ApplicationContext appContext, Guid id)
        {
            var alpha = CslaSafeConstructor(appContext);
            alpha.LoadProperty(IdPropertyInfo, id);
            alpha.LoadProperty(NamePropertyInfo, alpha.Id.ToString());

            return alpha;
        }
    }
}

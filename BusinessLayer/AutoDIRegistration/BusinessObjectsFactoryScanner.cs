using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer.AutoDIRegistration
{
    internal class BusinessObjectsFactoryScanner
    {
        internal BusinessObjectsFactoryScanner(Type scanInput)
        {
            ScanInput = scanInput;
        }

        private Type ScanInput { get; init; }

        internal IEnumerable<Type> DiscoverBusinessObjectFactories()
        {
            return ScanInput.Assembly.GetTypes()
                .Where(m => m.IsClass &&
                    !m.IsAbstract &&
                        m.IsAssignableTo(typeof(IBusinessObjectFactory)));
        }
    }
}

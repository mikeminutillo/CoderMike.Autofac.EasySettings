using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using Autofac.Builder;

namespace CoderMike.Autofac.EasySettings
{
    public class SettingsSource : IRegistrationSource
    {
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            var typedService = service as IServiceWithType;
            if (typedService != null && typedService.ServiceType.IsClass && typedService.ServiceType.Name.EndsWith("Settings"))
            {
                yield return RegistrationBuilder.ForDelegate(
                    (c, p) =>
                        {
                            var instance = Activator.CreateInstance(typedService.ServiceType);
                            c.Resolve<ISettingsInjector>().Inject(instance, c.Resolve<IEnumerable<ISettingsReader>>());
                            return instance;
                        }
                ).As(typedService.ServiceType)
                .CreateRegistration();
            }
        }

        public bool IsAdapterForIndividualComponents
        {
            get { return false; }
        }
    }
}

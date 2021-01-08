﻿using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Misc.ErplyIntegration.Factories;
using Nop.Plugin.Misc.ErplyIntegration.Services;

namespace Nop.Plugin.Misc.ErplyIntegration.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<ErplyApiFactory>().As<IErplyApiFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ErplyImportManager>().As<IErplyImportManager>().InstancePerLifetimeScope();
            builder.RegisterType<ErplyMappingService>().As<IErplyMappingService>().InstancePerLifetimeScope();
        }

        public int Order => 1;
    }
}

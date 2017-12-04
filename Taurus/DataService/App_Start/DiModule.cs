﻿using System.Web.Http.Dependencies;
using System.Web.Http.Filters;
using System.Web.Http.Validation;

using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.WebApi.Filter;
using Ninject.Web.WebApi.Validation;
using EF.Data;
using System.Data.Entity;

namespace DataService
{

    /// <summary>
    /// Defines the bindings and plugins of the WebApi extension.
    /// </summary>
    public class DiModule : NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            this.Bind<IClientRepository>().To<ClientRepository>();
            this.Bind<IAuthRepository>().To<AuthRepository>();
            this.Bind<ICompanyRepository>().To<CompanyRepository>();
            this.Bind<IMessageRepository>().To<MessageRepository>();
            this.Bind<IPropertyRepository>().To<PropertyRepository>();
            this.Bind<ISectionRepository>().To<SectionRepository>();
            this.Bind<IStorageRepository>().To<StorageRepository>();
            this.Bind<IWorkItemRepository>().To<WorkItemRepository>();
            this.Bind<IWorkItemTemplateRepository>().To<WorkItemTemplateRepository>();

            this.Bind<DbContext>().ToSelf().InRequestScope();

        }
    }
}
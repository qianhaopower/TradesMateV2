using System.Reflection;
using System.Web;
using System.Web.Http;
using DataService.AuthProviders;
using DataService.Infrastructure;
using DataService.Providers;
using EF.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace DataService
{
    public partial class Startup
    {
        public IKernel ConfigureNinject(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var kernel = CreateKernel();
            app.UseNinjectMiddleware(() => kernel)
                .UseNinjectWebApi(config);

            return kernel;
        }

        public IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }


    }

    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            RegisterServices();
        }

        private void RegisterServices()
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
            this.Bind<IEmailRepository>().To<EmailRepository>();

            //this.Bind<>().ToSelf().InRequestScope();
            //this.Bind<DbContext>().To<EFDbContext>().InRequestScope();
            //throw new System.Exception("My test exception");
            // this.Bind<ApplicationUserManager>().ToSelf();
            //this.Bind<IUserStore<ApplicationUser>>().To<ApplicationUserStore>();
            this.Bind<ApplicationUserManager>().ToMethod(
                c =>
                    HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()).InRequestScope();
            this.Bind<EFDbContext>().ToMethod(
                c =>
                    HttpContext.Current.GetOwinContext().Get<EFDbContext>()).InRequestScope();
            this.Bind<IOAuthAuthorizationServerOptions>()
                .To<MyOAuthAuthorizationServerOptions>();
            this.Bind<IOAuthAuthorizationServerProvider>()
                .To<SimpleAuthorizationServerProvider>();
            this.Bind<IAuthenticationTokenProvider>().To<SimpleRefreshTokenProvider>();
        }
    }
}



using DataService.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ninject;

namespace EF.Data
{
    public abstract class BaseRepository : IBaseRepository, IDisposable
    {
        protected  EFDbContext _ctx;

        protected UserManager<ApplicationUser> _userManager;


        protected BaseRepository(EFDbContext ctx, ApplicationUserManager manager)
        {
            _ctx = ctx;
            _userManager = manager ?? new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));
        }

        public void Dispose()
        {
        }

    }
}
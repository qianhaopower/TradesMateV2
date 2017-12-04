


using DataService.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace EF.Data
{
    public abstract class BaseRepository : IDisposable
    {
        protected EFDbContext _ctx;

        protected UserManager<ApplicationUser> _userManager;
        public BaseRepository(EFDbContext ctx)
        {
            _ctx = ctx;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

    }
}
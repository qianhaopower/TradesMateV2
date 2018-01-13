using DataService.Infrastructure;
using DataService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using Z.EntityFramework.Plus;

namespace EF.Data
{

    public class EmailRepository : BaseRepository, IEmailRepository
    {
        public EmailRepository(EFDbContext ctx) :base(ctx)
        {
            
        }
        public void Save(EmailHistory record)
        {
            _ctx.EmailHistories.Add(record);
            _ctx.SaveChanges();
        }
    }
}
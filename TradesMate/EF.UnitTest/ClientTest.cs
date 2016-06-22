using System;
using System.Collections.Generic;
using System.Data.Entity;
using EF.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF.UnitTest
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void ClientTestTest()
        {
            Database.SetInitializer<EFDbContext>(new DropCreateDatabaseAlways<EFDbContext>());
            using (var context = new EFDbContext())
            {

               // context.Database.Create();
                Client client = new Client
                {
                    FirstName = "Lee",
                    SurName = "Pinder",
                    Email = "raviendra@test.com",
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(client).State = EntityState.Added;
                context.SaveChanges();
            }
        }
    }
}

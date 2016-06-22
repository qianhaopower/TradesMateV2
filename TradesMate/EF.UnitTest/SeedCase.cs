using System;
using System.Collections.Generic;
using System.Data.Entity;
using EF.Data;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EF.UnitTest
{
    [TestClass]
    public class SeedCase
    {
        [TestMethod]
        public void ClientTestTest()
        {
            Database.SetInitializer<EFDbContext>(new CreateDatabaseIfNotExists<EFDbContext>());

            using (var context = new EFDbContext())
            {

                //Address address1 = new Address
                //{
                //    City = "Melbourne",
                //    Line1 = "46 Lincoln Drive",
                //    Line2 = "Cheltenham",
                //    PostCode = "3192",
                //    State = "VIC",
                //    Suburb ="Cheltenham",
                    

                //    AddedDate = DateTime.Now,
                //    ModifiedDate = DateTime.Now,
                //};

                //context.Entry(address1).State = EntityState.Added;
                ////context.SaveChanges();


                //Address address2 = new Address
                //{
                //    City = "Melbourne",
                //    Line1 = "18 Clayton Rd",
                //    Line2 = "Clayton",
                //    PostCode = "3168",
                //    State = "VIC",
                //    Suburb = "Clayton",


                //    AddedDate = DateTime.Now,
                //    ModifiedDate = DateTime.Now,
                //};

                //context.Entry(address2).State = EntityState.Added;
                ////context.SaveChanges();


                //client
                // context.Database.Create();
                Client client = new Client
                {
                    FirstName = "Lee",
                    SurName = "Pinder",
                    Email = "raviendra@test.com",
                  Address = new Address()
                  {
                      City = "Melbourne",
                      Line1 = "18 Clayton Rd",
                      Line2 = "Clayton",
                      PostCode = "3168",
                      State = "VIC",
                      Suburb = "Clayton",


                      AddedDate = DateTime.Now,
                      ModifiedDate = DateTime.Now,
                  },
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(client).State = EntityState.Added;
                context.SaveChanges();


                // context.Database.Create();
                Property property = new Property
                {
                    Name = "Hao's house",
                    Address = new Address() {
                        City = "Melbourne",
                        Line1 = "46 Lincoln Drive",
                        Line2 = "Cheltenham",
                        PostCode = "3192",
                        State = "VIC",
                        Suburb = "Cheltenham",


                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    Description = "Hao Qian's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",
                    ClientId = 1,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(property).State = EntityState.Added;
                //context.SaveChanges();


                Section sec = new Section
                {
                    Name = "Main Bedroom",
                    Description = "This is the north-facing main bedroom",
                    Property = property,


                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(sec).State = EntityState.Added;
               // context.SaveChanges();



                // context.Database.Create();
                Company company    = new Company
                {
                   Description = "TradesMate Software solutions. Providing best trads software",
                   Name = "TradesMate Soft",
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(company).State = EntityState.Added;
                //context.SaveChanges();


                // context.Database.Create();
                WorkItemTemplate item = new WorkItemTemplate
                {
                    Description = "Install power switch",
                    Name = "Gneral Power Switch Install",
                  Company = company,
                   TradeWorkType = TradeType.Electrician,
                   
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item).State = EntityState.Added;
                //context.SaveChanges();


                WorkItem workItem = new WorkItem
                {
                    Description = "Install power switch",
                    Name = "Gneral Power Switch Install",
                    Qauntity =4,
                    Section = sec,
                    TemplateRecord = item,
                    TradeWorkType = TradeType.Electrician,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(workItem).State = EntityState.Added;






                context.SaveChanges();
            }
        }
    }
}

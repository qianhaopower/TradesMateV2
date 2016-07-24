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
                #region client

                // context.Database.Create();
                Client clientLee = new Client
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
                    Properties = new List<Property>() {
                      new Property() {
                          Comment = "Lee\'s House",
                          Description = "This property is built in the mid 70s",
                          Condition = "Normal",
                          Narrative  = "Lee has not done major work on this house for 10 years",
                          Name = "Lee\'s House",
                          AddedDate = DateTime.Now,
                          ModifiedDate = DateTime.Now,
                          AddressId = null,
                      },
                      
                  },
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(clientLee).State = EntityState.Added;



                Client client2 = new Client
                {
                    FirstName = "Joe",
                    SurName = "Smith",
                    Email = "Smith@gmail.com",
                    MobileNumber = "0454112547",
                    Description = "Joe Smith has a lot of protential work at his house",
                    Address = new Address()
                    {
                        City = "Melbourne",
                        Line1 = "573 North Rd",
                        Line2 = "Springvale",
                        PostCode = "3156",
                        State = "VIC",
                        Suburb = "Springvale",


                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    Properties = new List<Property>() {
                      new Property() {
                          Comment = "Joe\'s House",
                          Description = "This property is built in the late 80s",
                          Condition = "Normal",
                          Narrative  = "The safety switch of the House is old and need to be replaced. ",
                          Name = "Joe\'s House",
                          AddedDate = DateTime.Now,
                          ModifiedDate = DateTime.Now,
                          AddressId = null,
                      },

                  },


                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(client2).State = EntityState.Added;




                Client client3 = new Client
                {
                    FirstName = "Lisa",
                    SurName = "Day",
                    Email = "lisa@yahoo.com",
                    Address = new Address()
                    {
                        City = "Sydney",
                        Line1 = "18 Church Rd",
                        Line2 = "Bellavista",
                        PostCode = "2154",
                        State = "NSW",
                        Suburb = "Bellavista",


                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(client3).State = EntityState.Added;



                Client client4 = new Client
                {
                    FirstName = "Kelly",
                    SurName = "Hilton",
                    Email = "Kelly.hilton@mail.com",
                    MobileNumber = "0454141589",
                    Address = new Address()
                    {
                        City = "Melbourne",
                        Line1 = "23 Kambrook Drive",
                        Line2 = "Caulfield",
                        PostCode = "3151",
                        State = "VIC",
                        Suburb = "Caulfield",


                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(client4).State = EntityState.Added;


                Client clien5 = new Client
                {
                    FirstName = "Simon",
                    SurName = "Bing",
                    Email = "simon.bing@gmail.com",
                    MobileNumber = "0425841235",
                    Address = new Address()
                    {
                        City = "Melbourne",
                        Line1 = "223 Burke Rd",
                        Line2 = "Malven",
                        PostCode = "3152",
                        State = "VIC",
                        Suburb = "Malvern",



                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(clien5).State = EntityState.Added;

                #endregion


                #region property
                // context.Database.Create();
                Property property = new Property
                {
                    Name = "Lee's house",
                    Address = new Address()
                    {
                        City = "Melbourne",
                        Line1 = "46 Lincoln Drive",
                        Line2 = "Cheltenham",
                        PostCode = "3192",
                        State = "VIC",
                        Suburb = "Cheltenham",


                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    Description = "Lee Pinder's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",
                    Client = clientLee,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(property).State = EntityState.Added;
                //context.SaveChanges();
                #endregion

                #region section

                Section sec = new Section
                {
                    Name = "Main Bedroom",
                    Description = "This is the north-facing main bedroom",
                    Property = property,
                    Type = "Living room",

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(sec).State = EntityState.Added;
                // context.SaveChanges();

                #endregion

                #region company
                // context.Database.Create();
                Company company = new Company
                {
                    Description = "TradesMate Software solutions. Providing best trads software",
                    Name = "TradesMate Soft",
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(company).State = EntityState.Added;
                //context.SaveChanges();
                #endregion

                #region workItem template
                // context.Database.Create();
                WorkItemTemplate item = new WorkItemTemplate
                {
                    Description = "Install power switch",
                    Name = "General Power Switch Install",
                    Company = company,
                    TradeWorkType = TradeType.Electrician,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item).State = EntityState.Added;


                WorkItemTemplate item2 = new WorkItemTemplate
                {
                  
                    Name = "Down Lignt Install",
                    Description =  " Brand name:      \r\n " +
                                   " Type:            \r\n " +
                                   " Cut out size:    \r\n " +
                                   " Globe type:      \r\n " +
                                   " Globe colour:    \r\n " +
                                   " Globe Intesity:  \r\n " +
                                   " Beam angle:      \r\n " ,
                    Company = company,
                    TradeWorkType = TradeType.Electrician,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item2).State = EntityState.Added;

                WorkItemTemplate item3 = new WorkItemTemplate
                {

                    Name = "Pedant Haning Light Install",
                    Description =  " Brand name:      \r\n " +
                                   " Type:            \r\n " +
                                   " Weight:    \r\n " +
                                   " Pendant length:      \r\n " +
                                   " Fixing to the ceiling:    \r\n " +
                                   " Globe type:  \r\n " +
                                   " Globe colour:      \r\n ",
                    Company = company,
                    TradeWorkType = TradeType.Electrician,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item3).State = EntityState.Added;

                WorkItemTemplate item4 = new WorkItemTemplate
                {

                    Name = "Surface Mounted Light Install",
                    Description = " Brand name:      \r\n " +
                                   " Type:            \r\n " +
                                   " Cut out size:    \r\n " +
                                   " Globe type:      \r\n " +
                                   " Globe colour:    \r\n " +
                                   " Globe Intesity:  \r\n " +
                                   " Beam angle:      \r\n ",
                    Company = company,
                    TradeWorkType = TradeType.Electrician,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item4).State = EntityState.Added;

                WorkItemTemplate item5 = new WorkItemTemplate
                {

                    Name = "Lighting point Install",
                    Description = " Brand name:      \r\n " +
                                   " Switching point:         \r\n " +
                                   " Type of power point:    \r\n " +
                                   " Maximum Rating:      \r\n " 
                                   ,
                    Company = company,
                    TradeWorkType = TradeType.Electrician,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item5).State = EntityState.Added;


                WorkItemTemplate item6 = new WorkItemTemplate
                {

                    Name = "Kitchen sink pipe Install",
                    Description = " Pipe name:      \r\n " +
                                   " Joint type:         \r\n " +
                                   " length:    \r\n " +
                                   " Rating:      \r\n "
                                   ,
                    Company = company,
                    TradeWorkType = TradeType.Plumber,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item6).State = EntityState.Added;

                WorkItemTemplate item7 = new WorkItemTemplate
                {

                    Name = "Tap Install",
                    Description = " Brand Name:      \r\n " +
                                   " Tap Type:         \r\n " +
                                   " Weight:    \r\n " +
                                   " Rating:      \r\n ",
                    Company = company,
                    TradeWorkType = TradeType.Plumber,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item7).State = EntityState.Added;


                WorkItemTemplate item8 = new WorkItemTemplate
                {

                    Name = "Sliding window fix",
                    Description = " Brand Name:      \r\n " +
                                   
                                   " Parts required:      \r\n ",
                    Company = company,
                    TradeWorkType = TradeType.Handyman,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(item8).State = EntityState.Added;



                //context.SaveChanges();
                #endregion

                #region workItem


                WorkItem workItem = new WorkItem
                {
                    Description = "Install power switch",
                    Name = "General Power Switch Install",
                    Quantity = 3,
                    Section = sec,
                    TemplateRecord = item,
                    TradeWorkType = TradeType.Electrician,

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(workItem).State = EntityState.Added;

                #endregion




                context.SaveChanges();
            }
        }
    }
}

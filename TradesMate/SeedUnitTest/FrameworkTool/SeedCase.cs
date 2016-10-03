using System;
using System.Collections.Generic;
using System.Data.Entity;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using EF.Data;
using DataService.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Linq;
using DataService.Models;

namespace EF.UnitTest
{
    [TestClass]
    public class SeedCase
    {
        const string Password123456 = "ADcrdCgS8Tnt1swRafCDcve2m68OwMHwOBUViq8BD516Fk+WYtDkNCptN19ZvoAlHQ==";

        [TestMethod]
        public void ApplicationSeed()
        {
            Database.SetInitializer<EFDbContext>(new DropCreateDatabaseAlways<EFDbContext>());

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
                    //Properties = new List<Property>() {
                    //  new Property() {
                    //      Comment = "Lee\'s House",
                    //      Description = "This property is built in the mid 70s",
                    //      Condition = "Normal",
                    //      Narrative  = "Lee has not done major work on this house for 10 years",
                    //      Name = "Lee\'s House",
                    //      AddedDate = DateTime.Now,
                    //      ModifiedDate = DateTime.Now,
                    //      AddressId = null,
                    //  },
                      
                //  },
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(clientLee).State = EntityState.Added;

                context.Clients.Add(clientLee);



                Client clientJoe = new Client
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
                  //  Properties = new List<Property>() {
                  //    new Property() {
                  //        Comment = "Joe\'s House",
                  //        Description = "This property is built in the late 80s",
                  //        Condition = "Normal",
                  //        Narrative  = "The safety switch of the House is old and need to be replaced. ",
                  //        Name = "Joe\'s House",
                  //        AddedDate = DateTime.Now,
                  //        ModifiedDate = DateTime.Now,
                  //        AddressId = null,
                  //    },

                  //},


                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(clientJoe).State = EntityState.Added;

                context.Clients.Add(clientJoe);


                Client clientLisa = new Client
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

                context.Entry(clientLisa).State = EntityState.Added;
                context.Clients.Add(clientLisa);


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
                context.Clients.Add(client4);

                Client client5 = new Client
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

                context.Entry(client5).State = EntityState.Added;
                context.Clients.Add(client5);

                #endregion

                #region User for clients




                foreach (var clientInst in context.Clients.Local.ToList())
                {
                    ApplicationUser userIns = new ApplicationUser
                    {
                        FirstName = clientInst.FirstName,
                        LastName = clientInst.SurName,
                        Email = clientInst.Email,
                        UserName = clientInst.FirstName.ToLower(),
                        JoinDate = DateTime.Now,
                        PasswordHash = Password123456,//123456
                        UserType = (int)UserType.Client,
                        Client = clientInst,

                    };

                    context.Entry(userIns).State = EntityState.Added;
                }



                // context.Entry(userLee).State = EntityState.Modified;


                #endregion

                #region property
                // context.Database.Create();
                Property propertyLee = new Property
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

                

                context.Entry(propertyLee).State = EntityState.Added;



                Property propertyJoe = new Property
                {
                    Name = "Joe's house",
                    Address = new Address()
                    {
                        City = "Geelong",
                        Line1 = "57 Dixon Drive",
                        Line2 = "Geelong",
                        PostCode = "4158",
                        State = "VIC",
                        Suburb = "Geelong",


                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    Description = "Joe Smith's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",
                    Client = clientJoe,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,

                };



                context.Entry(propertyLee).State = EntityState.Added;




                Property propertyLisa = new Property
                {
                    Name = "Lisa's house",
                    Address = new Address()
                    {
                        City = "Melbourne",
                        Line1 = "71 Nicholson St",
                        Line2 = "Brunswick",
                        PostCode = "3057",
                        State = "VIC",
                        Suburb = "Brunswick",


                        AddedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    },
                    Description = "Lisa Pinder's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",
                    Client = clientLisa,
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,

                };



                context.Entry(propertyLee).State = EntityState.Added;
        
                #endregion

                #region section

                Section sec = new Section
                {
                    Name = "Main Bedroom",
                    Description = "This is the north-facing main bedroom",
                    Property = propertyLee,
                    Type = "Living room",

                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                context.Entry(sec).State = EntityState.Added;
                // context.SaveChanges();

                #endregion

                #region company
                // context.Database.Create();
                Company companyTradesMate = new Company
                {
                    Description = "TradesMate Software solutions. Providing best trades software",
                    Name = "TradesMate Soft",
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                companyTradesMate.Properties.Add(propertyLee);
                companyTradesMate.Properties.Add(propertyJoe);

                context.Entry(companyTradesMate).State = EntityState.Added;



                Company companyBilly = new Company
                {
                    Description = "Billy's trade company . Providing best trades software",
                    Name = "Billy's trade",
                    AddedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };

                companyBilly.Properties.Add(propertyLisa);
                companyBilly.Properties.Add(propertyLee);
                context.Entry(companyBilly).State = EntityState.Added;


                //context.SaveChanges();
                #endregion

                #region company users

                ApplicationUser userOleg = new ApplicationUser
                {
                    FirstName = "Oleg",
                    LastName = "Lien",
                    Email = "oleg.lien@tradsmate.com",
                    UserName = "Oleg".ToLower(),
                    JoinDate = DateTime.Now,
                    PasswordHash = Password123456,//123456
                    UserType = (int)UserType.Trade,
                    
                };

                context.Entry(userOleg).State = EntityState.Added;

                ApplicationUser userRoger = new ApplicationUser
                {
                    FirstName = "Roger",
                    LastName = "Yearwood",
                    Email = "roger.yearwood@tradsmate.com",
                    UserName = "Roger".ToLower(),
                    JoinDate = DateTime.Now,
                    PasswordHash = Password123456,//123456
                    UserType = (int)UserType.Trade,

                };

                context.Entry(userRoger).State = EntityState.Added;

                ApplicationUser userRalph = new ApplicationUser
                {
                    FirstName = "Ralph",
                    LastName = "Carrow",
                    Email = "ralph.carrow@tradsmate.com",
                    UserName = "Ralph".ToLower(),
                    JoinDate = DateTime.Now,
                    PasswordHash = Password123456,//123456
                    UserType = (int)UserType.Trade,

                };

                context.Entry(userRalph).State = EntityState.Added;

                ApplicationUser userBilly = new ApplicationUser
                {
                    FirstName = "Billy",
                    LastName = "Bowen",
                    Email = "billy.bowen@billy.com",
                    UserName = "Billy".ToLower(),
                    JoinDate = DateTime.Now,
                    PasswordHash = Password123456,//123456
                    UserType = (int)UserType.Trade,

                };

                context.Entry(userBilly).State = EntityState.Added;

                ApplicationUser userElwood = new ApplicationUser
                {
                    FirstName = "Elwood",
                    LastName = "Olin",
                    Email = "elwood.olin@billy.com",
                    UserName = "Elwood".ToLower(),
                    JoinDate = DateTime.Now,
                    PasswordHash = Password123456,//123456
                    UserType = (int)UserType.Trade,

                };

                context.Entry(userElwood).State = EntityState.Added;

                //set up companyId and Role
                #endregion


                #region workItem template
                // context.Database.Create();
                WorkItemTemplate item = new WorkItemTemplate
                {
                    Description = "Install power switch",
                    Name = "General Power Switch Install",
                    Company = companyTradesMate,
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
                    Company = companyTradesMate,
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
                    Company = companyTradesMate,
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
                    Company = companyTradesMate,
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
                    Company = companyTradesMate,
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
                    Company = companyTradesMate,
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
                    Company = companyTradesMate,
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
                    Company = companyTradesMate,
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


                //ClientApplicaiton app1 = new ClientApplicaiton
                //{
                //    Id = "ngAuthApp",
                //    Secret = Helper.GetHash("abc@123"),
                //    Name = "AngularJS front-end Application",
                //    ApplicationType = AuthenticationService.API.Models.ApplicationTypes.JavaScript,
                //    Active = true,
                //    RefreshTokenLifeTime = 7200,
                //    AllowedOrigin = "*"
                //};

                //context.Entry(app1).State = EntityState.Added;

                //ClientApplicaiton app2 = new ClientApplicaiton
                //{
                //    Id = "consoleApp",
                //    Secret = Helper.GetHash("123@abc"),
                //    Name = "Console Application",
                //    ApplicationType = AuthenticationService.API.Models.ApplicationTypes.NativeConfidential,
                //    Active = true,
                //    RefreshTokenLifeTime = 14400,
                //    AllowedOrigin = "*"
                //};

               // context.Entry(app2).State = EntityState.Added;

                context.SaveChanges();

                userBilly.CompanyId = context.Companies.First(p => p.Name == "Billy's trade").Id;
                userElwood.CompanyId = context.Companies.First(p => p.Name == "Billy's trade").Id;

                userOleg.CompanyId = context.Companies.First(p => p.Name == "TradesMate Soft").Id;
                userRoger.CompanyId = context.Companies.First(p => p.Name == "TradesMate Soft").Id;
                userRalph.CompanyId = context.Companies.First(p => p.Name == "TradesMate Soft").Id;


                context.SaveChanges();

          
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                var user = new ApplicationUser()
                {
                    UserName = "SuperUser",
                    Email = "haoqian@tradesmate.com",
                    EmailConfirmed = true,
                    FirstName = "Hao",
                    LastName = "Qian",

                    JoinDate = DateTime.Now.AddYears(-3)//fake that I joined 3 years ago
                };

                manager.Create(user, "123456");

                if (roleManager.Roles.Count() == 0)
                {
                    roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                    roleManager.Create(new IdentityRole { Name = "User" });
                }

                var adminUser = manager.FindByName("SuperUser");

                manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });


                var userOlegNew = manager.FindByName("oleg");

                manager.AddToRoles(userOlegNew.Id, new string[] { "Admin" });


                var userBillyNew = manager.FindByName("billy");

                manager.AddToRoles(userBillyNew.Id, new string[] { "Admin" });

                context.SaveChanges();
            }
        }

        //[TestMethod]
        //public void AuthSeed()
        //{
        //    Database.SetInitializer<AuthContext>(new CreateDatabaseIfNotExists<AuthContext>());

        //    using (var context = new AuthContext())
        //    {

        //        var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AuthContext()));

        //        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext()));

        //        var user = new ApplicationUser()
        //        {
        //            UserName = "SuperUser",
        //            Email = "haoqian@tradesmate.com",
        //            EmailConfirmed = true,
        //            FirstName = "Hao",
        //            LastName = "Qian",

        //            JoinDate = DateTime.Now.AddYears(-3)//fake that I joined 3 years ago
        //        };

        //        manager.Create(user, "123456");

        //        if (roleManager.Roles.Count() == 0)
        //        {
        //            roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
        //            roleManager.Create(new IdentityRole { Name = "Admin" });
        //            roleManager.Create(new IdentityRole { Name = "User" });
        //        }

        //        var adminUser = manager.FindByName("SuperUser");

        //        manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });



        //        context.SaveChanges();
        //    }
        //}
    }
}

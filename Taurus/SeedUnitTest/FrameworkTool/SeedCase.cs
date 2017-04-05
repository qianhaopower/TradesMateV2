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
                    LastName = "Pinder",
                    Email = "raviendra@test.com",
                    Address = new Address()
                    {
                        City = "Melbourne",
                        Line1 = "18 Clayton Rd",
                        Line2 = "Clayton",
                        PostCode = "3168",
                        State = "VIC",
                        Suburb = "Clayton",


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
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
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientLee).State = EntityState.Added;

                context.Clients.Add(clientLee);



                Client clientJoe = new Client
                {
                    FirstName = "Joe",
                    LastName = "Smith",
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


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
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


                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientJoe).State = EntityState.Added;

                context.Clients.Add(clientJoe);


                Client clientLisa = new Client
                {
                    FirstName = "Lisa",
                    LastName = "Day",
                    Email = "lisa@yahoo.com",
                    Address = new Address()
                    {
                        City = "Sydney",
                        Line1 = "18 Church Rd",
                        Line2 = "Bellavista",
                        PostCode = "2154",
                        State = "NSW",
                        Suburb = "Bellavista",


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    },
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientLisa).State = EntityState.Added;
                context.Clients.Add(clientLisa);


                Client clientKelly = new Client
                {
                    FirstName = "Kelly",
                    LastName = "Hilton",
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


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    },
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientKelly).State = EntityState.Added;
                context.Clients.Add(clientKelly);

                Client clientSimon = new Client
                {
                    FirstName = "Simon",
                    LastName = "Bing",
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



                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    },
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientSimon).State = EntityState.Added;
                context.Clients.Add(clientSimon);

                #endregion

                #region User for clients




                foreach (var clientInst in context.Clients.Local.ToList())
                {
                    ApplicationUser userIns = new ApplicationUser
                    {
                        FirstName = clientInst.FirstName,
                        LastName = clientInst.LastName,
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


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    },
                    Description = "Lee Pinder's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",                
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    
                };
                context.Entry(propertyLee).State = EntityState.Added;

                ClientProperty clientPropertyLee = new ClientProperty
                {
                    Client = clientLee,
                    Property = propertyLee,
                    Confirmed = true,
                    Role = ClientRole.Owner,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientPropertyLee).State = EntityState.Added;

                ClientProperty clientPropertyKelly = new ClientProperty
                {
                    Client = clientKelly,
                    Property = propertyLee,
                    Confirmed = true,
                    Role = ClientRole.CoOwner,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientPropertyKelly).State = EntityState.Added;



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


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    },
                    Description = "Joe Smith's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",
                   
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,

                };
                context.Entry(propertyLee).State = EntityState.Added;

                ClientProperty clientPropertyJoe = new ClientProperty
                {
                    Client = clientJoe,
                    Property = propertyJoe,
                    Confirmed = true,
                    Role = ClientRole.Owner,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientPropertyJoe).State = EntityState.Added;




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


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    },
                    Description = "Lisa Pinder's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                 

                };
                context.Entry(propertyLee).State = EntityState.Added;

                ClientProperty clientPropertyLisa = new ClientProperty
                {
                    Client = clientLisa,
                    Property = propertyLisa,
                    Confirmed = true,
                    Role = ClientRole.Owner,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientPropertyLisa).State = EntityState.Added;



                Property propertySimon = new Property
                {
                    Name = "Simon's house",
                    Address = new Address()
                    {
                        City = "Bayswater",
                        Line1 = "64 Kings St",
                        Line2 = "Bayswater",
                        PostCode = "3159",
                        State = "VIC",
                        Suburb = "Bayswater",


                        AddedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                    },
                    Description = "Simon Bing's House",
                    Condition = "This house is in good condition",
                    Narrative = "This house hot water service needs update",
                    Comment = "",
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,

                };
                context.Entry(propertySimon).State = EntityState.Added;

                ClientProperty clientPropertySimon = new ClientProperty
                {
                    Client = clientSimon,
                    Property = propertySimon,
                    Confirmed = true,
                    Role = ClientRole.Owner,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(clientPropertySimon).State = EntityState.Added;

                #endregion

                #region section

                Section sec = new Section
                {
                    Name = "Main Bedroom",
                    Description = "This is the north-facing main bedroom",
                    Property = propertyLee,
                    Type = "Living room",

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(companyTradesMate).State = EntityState.Added;

                CompanyService tradesMateServiceElec = new CompanyService {
                    Company = companyTradesMate,
                    Type = TradeType.Electrician,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(tradesMateServiceElec).State = EntityState.Added;

                CompanyService tradesMateServiceHandy = new CompanyService
                {
                    Company = companyTradesMate,
                    Type = TradeType.Handyman,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(tradesMateServiceHandy).State = EntityState.Added;

                CompanyService tradesMateServiceBuilder = new CompanyService
                {
                    Company = companyTradesMate,
                    Type = TradeType.Builder,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(tradesMateServiceBuilder).State = EntityState.Added;



                PropertyCompany propertyCompanyJoe = new PropertyCompany
                {
                    Property = propertyJoe,
                    Company = companyTradesMate,
                     AddedDateTime = DateTime.Now,
                      ModifiedDateTime = DateTime.Now,
                };
                context.Entry(propertyCompanyJoe).State = EntityState.Added;

                PropertyCompany propertyCompanyLee = new PropertyCompany
                {
                    Property = propertyLee,
                    Company = companyTradesMate,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(propertyCompanyLee).State = EntityState.Added;

                PropertyCompany propertyCompanySimon = new PropertyCompany
                {
                    Property = propertySimon,
                    Company = companyTradesMate,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(propertyCompanySimon).State = EntityState.Added;


                Company companyBilly = new Company
                {
                    Description = "Billy's trade company . Providing best trades software",
                    Name = "Billy's trade",
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(companyBilly).State = EntityState.Added;


                CompanyService billyService = new CompanyService
                {
                    Company = companyBilly,
                    Type = TradeType.Electrician,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(billyService).State = EntityState.Added;



                PropertyCompany propertyCompanyLisa = new PropertyCompany
                {
                    Property = propertyLisa,
                    Company = companyBilly,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(propertyCompanyLisa).State = EntityState.Added;

                PropertyCompany propertyCompanyLeeNo2 = new PropertyCompany
                {
                    Property = propertyLee,
                    Company = companyBilly,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(propertyCompanyLeeNo2).State = EntityState.Added;


                //context.SaveChanges();
                #endregion

                #region company users

                Member memberOleg = new Member
                {
                    FirstName = "Oleg",
                    LastName = "Lien",
                    Email = "oleg.lien@tradsmate.com",
                    ModifiedDateTime = DateTime.Now,
                    AddedDateTime = DateTime.Now,
                    
                };

                context.Entry(memberOleg).State = EntityState.Added;
                CompanyMember cmOleg = new CompanyMember
                {
                    Member = memberOleg,
                    Company = companyTradesMate,
                    Role = CompanyRole.Admin,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(cmOleg).State = EntityState.Added;



                Member memberRoger = new Member
                {
                    FirstName = "Roger",
                    LastName = "Yearwood",
                    Email = "roger.yearwood@tradsmate.com",
                    ModifiedDateTime = DateTime.Now,
                    AddedDateTime = DateTime.Now,



                };
                context.Entry(memberRoger).State = EntityState.Added;
                CompanyMember cmRoger = new CompanyMember
                {
                    Member = memberRoger,
                    Company = companyTradesMate,
                    Role = CompanyRole.Default,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(cmRoger).State = EntityState.Added;








                Member memberRalph = new Member
                {
                    FirstName = "Ralph",
                    LastName = "Carrow",
                    Email = "ralph.carrow@tradsmate.com",
                    ModifiedDateTime = DateTime.Now,
                    AddedDateTime = DateTime.Now,


                };
                context.Entry(memberRalph).State = EntityState.Added;
                CompanyMember cmRalph = new CompanyMember
                {
                    Member = memberRalph,
                    Company = companyTradesMate,
                    Role = CompanyRole.Default,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(memberRalph).State = EntityState.Added;





                Member memberSonny = new Member
                {
                    FirstName = "Stonny",
                    LastName = "Getchell",
                    Email = "Stonny.Getchell@tradsmate.com",
                    ModifiedDateTime = DateTime.Now,
                    AddedDateTime = DateTime.Now,


                };
                context.Entry(memberSonny).State = EntityState.Added;
                CompanyMember cmStonny = new CompanyMember
                {
                    Member = memberSonny,
                    Company = companyTradesMate,
                    Role = CompanyRole.Contractor,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(cmStonny).State = EntityState.Added;



                Member memberTaylor = new Member
                {
                    FirstName = "Taylor",
                    LastName = "Diniz",
                    Email = "Taylor.Diniz@tradsmate.com",
                    ModifiedDateTime = DateTime.Now,
                    AddedDateTime = DateTime.Now,


                };
                context.Entry(memberTaylor).State = EntityState.Added;
                CompanyMember cmTaylor = new CompanyMember
                {
                    Member = memberTaylor,
                    Company = companyTradesMate,
                    Role = CompanyRole.Contractor,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(cmTaylor).State = EntityState.Added;




                Member membberBilly = new Member
                {
                    FirstName = "Billy",
                    LastName = "Bowen",
                    Email = "billy.bowen@billy.com",
                    ModifiedDateTime = DateTime.Now,
                    AddedDateTime = DateTime.Now,



                };
                context.Entry(membberBilly).State = EntityState.Added;
                CompanyMember cmBilly = new CompanyMember
                {
                    Member = membberBilly,
                    Company = companyBilly,
                    Role = CompanyRole.Admin,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(cmBilly).State = EntityState.Added;




                Member memberElwood = new Member
                {
                    FirstName = "Elwood",
                    LastName = "Olin",
                    Email = "elwood.olin@billy.com",
                    ModifiedDateTime = DateTime.Now,
                    AddedDateTime = DateTime.Now,

                };
                context.Entry(memberElwood).State = EntityState.Added;
                CompanyMember cmElwood = new CompanyMember
                {
                    Member = memberElwood,
                    Company = companyBilly,
                    Role = CompanyRole.Default,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(cmElwood).State = EntityState.Added;



                CompanyMember cmTaylorNo2 = new CompanyMember
                {
                    Member = memberTaylor,
                    Company = companyBilly,
                    Role = CompanyRole.Contractor,
                    Confirmed = true,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(cmTaylorNo2).State = EntityState.Added;




                #endregion

                #region member for companies

                foreach (var memberInst in context.Members.Local.ToList())
                {
                    ApplicationUser userIns = new ApplicationUser
                    {
                        FirstName = memberInst.FirstName,
                        LastName = memberInst.LastName,
                        Email = memberInst.Email,
                        UserName = memberInst.FirstName.ToLower(),
                        JoinDate = DateTime.Now,
                        PasswordHash = Password123456,//123456
                        UserType = UserType.Trade,
                        Member = memberInst,

                    };

                    context.Entry(userIns).State = EntityState.Added;
                }


                #endregion

                #region property allocation

                PropertyAllocation allocationSonnyTradesMate = new PropertyAllocation
                {
                    Property = propertySimon,
                    CompanyMember = cmStonny,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(allocationSonnyTradesMate).State = EntityState.Added;
                PropertyAllocation allocationTayloyBilly = new PropertyAllocation
                {
                    Property = propertyLee,
                    CompanyMember = cmTaylorNo2,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,


                };
                context.Entry(allocationTayloyBilly).State = EntityState.Added;
                PropertyAllocation allocationTayloyTradesMate = new PropertyAllocation
                {
                    Property = propertyJoe,
                    CompanyMember = cmTaylor,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };
                context.Entry(allocationTayloyTradesMate).State = EntityState.Added;
                #endregion

                #region workItem template
                // context.Database.Create();
                WorkItemTemplate item = new WorkItemTemplate
                {
                    Description = "Install power switch",
                    Name = "General Power Switch Install",
                    Company = companyTradesMate,
                    TradeWorkType = TradeType.Electrician,

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(item7).State = EntityState.Added;


                WorkItemTemplate item8 = new WorkItemTemplate
                {

                    Name = "Sliding window fix",
                    Description = " Brand Name:      \r\n " +
                                   
                                   " Parts required:      \r\n ",
                    Company = companyTradesMate,
                    TradeWorkType = TradeType.Handyman,

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
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

                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                };

                context.Entry(workItem).State = EntityState.Added;

                #endregion


                context.SaveChanges();

          
                //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


                //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //var user = new ApplicationUser()
                //{
                //    UserName = "SuperUser",
                //    Email = "haoqian@tradesmate.com",
                //    EmailConfirmed = true,
                //    FirstName = "Hao",
                //    LastName = "Qian",

                //    JoinDate = DateTime.Now.AddYears(-3)//fake that I joined 3 years ago
                //};

                //manager.Create(user, "123456");

                //if (roleManager.Roles.Count() == 0)
                //{
                //    roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                //    roleManager.Create(new IdentityRole { Name = "Admin" });
                //    roleManager.Create(new IdentityRole { Name = "User" });
                //}

                //var adminUser = manager.FindByName("SuperUser");

                //manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });


                //var userOlegNew = manager.FindByName("oleg");

                //manager.AddToRoles(userOlegNew.Id, new string[] { "Admin" });


                //var userBillyNew = manager.FindByName("billy");

                //manager.AddToRoles(userBillyNew.Id, new string[] { "Admin" });

               // context.SaveChanges();
            }
        }

      
    }
}

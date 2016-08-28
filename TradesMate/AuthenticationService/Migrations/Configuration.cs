//namespace AngularJSAuthentication.API.Migrations
//{
//    using AuthenticationService.API.Entities;
//    using System;
//    using System.Collections.Generic;
//    using System.Data.Entity;
//    using System.Data.Entity.Migrations;
//    using System.Linq;

//    internal sealed class Configuration : DbMigrationsConfiguration<AuthenticationService.API.AuthContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = false;
//        }

//        protected override void Seed(AuthenticationService.API.AuthContext context)
//        {
//            if (context.ClientApplications.Count() > 0)
//            {
//                return;
//            }

//            context.ClientApplications.AddRange(BuildClientsList());
//            context.SaveChanges();
//        }

//        private static List<ClientApplicaiton> BuildClientsList()
//        {

//            List<ClientApplicaiton> ClientsList = new List<ClientApplicaiton> 
//            {
//                new ClientApplicaiton
//                { Id = "ngAuthApp", 
//                    Secret= AuthenticationService.API.Helper.GetHash("abc@123"), 
//                    Name="AngularJS front-end Application", 
//                    ApplicationType =  AuthenticationService.API.Models.ApplicationTypes.JavaScript, 
//                    Active = true, 
//                    RefreshTokenLifeTime = 7200, 
//                    AllowedOrigin = "http://ngauthenticationweb.azurewebsites.net"
//                },
//                new ClientApplicaiton
//                { Id = "consoleApp", 
//                    Secret= AuthenticationService.API.Helper.GetHash("123@abc"), 
//                    Name="Console Application", 
//                    ApplicationType =AuthenticationService.API.Models.ApplicationTypes.NativeConfidential, 
//                    Active = true, 
//                    RefreshTokenLifeTime = 14400, 
//                    AllowedOrigin = "*"
//                }
//            };

//            return ClientsList;
//        }
//    }
//}

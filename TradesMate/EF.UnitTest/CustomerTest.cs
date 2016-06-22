//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using EF.Data;
//using EF.Data;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace EF.UnitTest
//{
//    [TestClass]
//    public class CustomerTest
//    {
//        [TestMethod]
//        public void CustomerOrderTest()
//        {
//             Database.SetInitializer<EFDbContext>(new CreateDatabaseIfNotExists<EFDbContext>());
//             using (var context = new EFDbContext())
//             {
//                 context.Database.Create();
//                 Customer customer = new Customer
//                                     {
//                                         Name = "Raviendra",
//                                         Email = "raviendra@test.com",
//                                         AddedDate = DateTime.Now,
//                                         ModifiedDate = DateTime.Now,
                                      
//                                         Orders = new List<Order>{
//                                            new Order
//                                            {
//                                                Quanatity =12,
//                                                Price =15,
//                                                AddedDate = DateTime.Now,
//                                                ModifiedDate = DateTime.Now,
                                               
//                                            },
//                                            new Order
//                                            {
//                                                Quanatity =10,
//                                                Price =25,
//                                                AddedDate = DateTime.Now,
//                                                ModifiedDate = DateTime.Now,
                                                 
//                                            }
//                                        }
//                                     };
//                 context.Entry(customer).State = System.Data.EntityState.Added;
//                 context.SaveChanges();
//             }
//        }
//    }
//}

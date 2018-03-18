
using DataService.Infrastructure;
using DataService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace EF.Data
{

    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        #region messages
        private const string AssignDefaultRoleMessage = "Dear {0}, You are assigned default role in {1}. Now you can view all {1}'s properties and related works.";

        //XXX(Admin name) want to assign you defaut role in YYY(company name), if you accept you will become contractor in ZZZ,WWW and XXX(other company name).
        private const string AssignDefaultRoleRequestMessage = "Dear {0}, {1} want to assign you defaut role, if you accept your role in {2} will become contractor.";



        //XXX(Admin name) has assigned you contractor role in YYY(company name), now you can view YYY's properties and related works allocated to you.
        private const string AssignContractorRoleMessage = "Dear {0}, You are assigned contractor role in {1}. Now you can view {1}'s properties allocated to you.";

        //XXX(Admin name) has invited you to join YYY(company name).
        private const string InviteJoinCompanyRequestMessage = "Dear {0}, you are invited to join {1}. You can see {1}'s properties if accept.";

        //XXX(name) has invited you to join YYY(company name).
        private const string InviteClientJoinCompanyRequestMessage = "Dear {0}, {1} wants to list you as client. {1} will be able to allocate work to your proeprty if accept.";

        #endregion
        private readonly IAuthRepository _authRepo;
        public CompanyRepository(EFDbContext ctx, ApplicationUserManager manager, IAuthRepository authRepo) :base(ctx, manager)
        {
            _authRepo = authRepo;
        }

        public IQueryable<Company> GetAllCompanies()
        {
            return _ctx.Companies.Include(p => p.CompanyServices).AsQueryable();
        }
        

        public void CreateInviteToCompanyRequest(string userName, InviteClientModel model)
        {
            var companyId = GetCompanyForUser(userName).Id;
            var alreadyClientIds = (
                from client in _ctx.Clients
                join cp in _ctx.ClientProperties on client.Id equals cp.ClientId
                join pc in _ctx.PropertyCompanies on cp.PropertyId equals pc.PropertyId
                where pc.CompanyId == companyId

                select client.Id
            );
            if (alreadyClientIds.Contains(model.ClientId))
            {
                
                throw new Exception("Already a client for company, cannot invite again");
            }


             GenerateAddClientToCompany(model.ClientId, companyId, model.Text);
        }

        public void CreateJoinCompanyRequest(string userName, InviteMemberModel model)
        {
            var companyId = GetCompanyFoAdminUser(userName).Id;
            var otherCompanyInfo = GetMemberInfoOutsideCompany(companyId, model.MemberId);
            var inOtherCompanyAsAdmin = otherCompanyInfo.Any(p => p.CompanyMember.Role == CompanyRole.Admin);
            if (inOtherCompanyAsAdmin)
            {
                //people in other company is admin, as we cannot remove the admin role from the other company, set default role here is not allowed.
                // throw new Exception("Member is the admin of other company, cannot assign default role in this company.");
                throw new Exception( "Member is the admin of other company, cannot invite.");
            }
            GenerateAddMemberToCompany(model.MemberId, companyId, model.Text, CompanyRole.Contractor);//for now by default contractor.
        }

        public IEnumerable<MemberModel> GetMemberByUserName(string userName, int? memberId = null)
        {
            var companyId = GetCompanyFoAdminUser(userName).Id;
            var result = GetMemberByCompanyIdQuery(companyId, memberId).ToList();

            var allCompanyServices = _ctx.Companies.First(p => p.Id == companyId).CompanyServices.Select(p => p.Type).ToList();
            return result.Select(p => new MemberModel
            {
                FirstName = p.Member.FirstName,
                LastName = p.Member.LastName,
                Email = p.Member.Email,
                MemberRole = p.CompanyMember.Role.ToString(),
                MemberId = p.Member.Id,
                Username = p.User.UserName,
                AllowedTradeTypes = p.CompanyMember.Role == CompanyRole.Admin ? allCompanyServices : p.CompanyMember.AllowedTradeTypes,//admin always have all service for the company
            });

        }

        public IQueryable<Property> GetCompanyProperties(int companyId)
        {
            // get the company that this property has been assigned to.
            IQueryable<Property> properties = _ctx.Companies.Where(p => p.Id == companyId).SelectMany(p => p.PropertyCompanies).Select(p => p.Property);
            return properties;


        }

        public void UpdateCompany(CompanyModel companyModel)
        {
            // get the company that this property has been assigned to.
            var company = _ctx.Companies.First(p => p.Id == companyModel.CompanyId);

            if (company == null)
            {
                throw new Exception("Cannot find company");
            }
            company.Description = companyModel.Description;
            company.Name = companyModel.CompanyName;
            company.CreditCard = companyModel.CreditCard;
            company.AddressString = companyModel.Address;
            company.ABN = companyModel.ABN;
            company.Website = companyModel.Website;

            if (!companyModel.TradeTypes.Any())
            {
                throw new Exception("At least one service type is required");
            }
            _ctx.CompanyServices.Where(p => p.CompanyId == company.Id
             ).Delete();// delete all that is not listed in the new types

            // add new ones, not too many so we can just delete and add new.
            companyModel.TradeTypes.ForEach(p => {
                CompanyService cs = new CompanyService()
                {
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    Company = company,
                    Type = p,
                };
                _ctx.Entry(cs).State = EntityState.Added;
            });


            _ctx.Entry(company).State = EntityState.Modified;

            _ctx.SaveChanges();
        }


        public async Task RemoveMemberFromCompnay(string userName, int memberId)
        {
            var companyId = this.GetCompanyFoAdminUser(userName).Id;
            var error = await RemoveMemberValidation(userName, companyId, memberId);
            if (string.IsNullOrEmpty(error))
            {
                DoRemoveCompanyMember(companyId, memberId);
            }
            else
            {
                throw new Exception(error);
            }
        }

        private void DoRemoveCompanyMember(int companyId, int memberId)
        {
            var info = this.GetMemberByCompanyIdQuery(companyId, memberId).ToList().FirstOrDefault();
            if (info.CompanyMember.Role == CompanyRole.Contractor)
            {
                //remove all allocation

                _ctx.PropertyAllocations.Where(p => p.CompanyMemberId == info.CompanyMember.Id).Delete() ;
            }

            _ctx.CompanyMembers.Remove(info.CompanyMember);

            _ctx.SaveChanges();

        }

        private async Task<string> RemoveMemberValidation(string userName, int companyId, int memberId)
        {
            
            var isUserAdminTask = await _authRepo.IsUserAdminAsync(userName);

            // For Task (not Task<T>): will block until the task is completed...
            //isUserAdminTask.RunSynchronously();
            if (isUserAdminTask == false)
            {
                return "Only admin can remove member";

            }

            var info = this.GetMemberByCompanyIdQuery(companyId, memberId).ToList().FirstOrDefault();

            if (info.CompanyMember.Role == CompanyRole.Admin)
            {
                return "Admin member cannot be removed from company";
            }

            return string.Empty;

        }

        public MessageType? UpdateCompanyMemberRole(string userName, int memberId, string role)
        {
            MessageType? messageType = null;
            var error =  UpdateRoleValidation(userName, memberId, role, out messageType);

            if (string.IsNullOrEmpty(error))
            {
                
                var companyId = GetCompanyFoAdminUser(userName).Id;
                CompanyRole roleParsed;
                bool roleValid = Enum.TryParse<CompanyRole>(role, out roleParsed);
                switch (messageType)
                {
                    //here we generate message to user. 
                    case MessageType.AssignDefaultRole:
                        GenerateAssignDefaultRoleMessage(memberId, companyId);
                        //let it happen, no need to wait
                        DoUpdateCompanyMemberRole( companyId,memberId, roleParsed);
                        break;
                    case MessageType.AssignContractorRole:
                        GenerateAssignContractorRoleMessage(memberId, companyId);
                        DoUpdateCompanyMemberRole( companyId, memberId, roleParsed);
                        //let it happen, no need to wait
                        break;
                    case MessageType.AssignDefaultRoleRequest:// need wait for the request's response
                        GenerateAssignDefaultRoleRequestMessage(memberId, companyId);
                        break;               
                }
                return messageType;
            }
            else
            {
                throw new Exception(error);
            }


            //if (string.IsNullOrEmpty(error))
            //{
            //    CompanyRole roleParsed;
            //    bool roleValid = Enum.TryParse<CompanyRole>(role, out roleParsed);
            //    return DoUpdateCompanyMemberRole(userName, memberId, roleParsed);
            //}
            //else
            //{
            //    throw new Exception(error);
            //}

        }


        private  string UpdateRoleValidation(string userName, int memberId, string role, out MessageType? messageType)
        {
            messageType = null;
            var isUserAdminTask =  _authRepo.isUserAdmin(userName);

            // For Task (not Task<T>): will block until the task is completed...
            //isUserAdminTask.RunSynchronously();
            if (isUserAdminTask == false)
            {
                return "Only admin can update company members role";

            }

            CompanyRole roleParsed;
            bool roleValid = Enum.TryParse<CompanyRole>(role, out roleParsed);
            if (roleValid == false)
            {
                return string.Format("{0} is not a valid role name", role);
            }

            if (roleParsed == CompanyRole.Admin)
            {
                return string.Format("Cannot assign Admin role");
            }

            var companyId = GetCompanyFoAdminUser(userName).Id;
            var oldRole = _ctx.CompanyMembers.Where(p => p.CompanyId == companyId && p.MemberId == memberId).First().Role;

            if (oldRole == CompanyRole.Admin)
            {
                return string.Format("Admin role cannot change");
            }

            if (oldRole == roleParsed)
            {
                return string.Format("Already have role {0}", oldRole);
            }
            //up to here can only be default -> contractor or contractor -> default
            if (roleParsed == CompanyRole.Contractor)
            {
                //default -> contractor case, we need delete the allocation later, all good here. 
                messageType = MessageType.AssignContractorRole;
            }

            if (roleParsed == CompanyRole.Default)
            {
                var otherCompanyInfo = GetMemberInfoOutsideCompany(companyId, memberId);
                var inOtherCompanyAsAdmin = otherCompanyInfo.Any(p => p.CompanyMember.Role == CompanyRole.Admin);
                if (inOtherCompanyAsAdmin)
                {
                    //people in other company is admin, as we cannot remove the admin role from the other company, set default role here is not allowed.
                    // throw new Exception("Member is the admin of other company, cannot assign default role in this company.");
                    return "Member is the admin of other company, cannot assign default role in this company.";
                }

                var inOtherCompanyAsDefault = otherCompanyInfo.Any(p => p.CompanyMember.Role == CompanyRole.Default);//lost default role in other company

                if (inOtherCompanyAsDefault)
                {
                    //if yes, mark him/her as contractor for all other company, ask first. 

                    //request/response 
                    messageType = MessageType.AssignDefaultRoleRequest;
                    if (CheckIfThereIsWaitingDefaultRoleRequestMessage(memberId, companyId))
                    {
                        return string.Format("There is already a same pending request, Please wait for member's response");
                    }

                }
                else
                {
                    messageType = MessageType.AssignDefaultRole;//do not need request
                }
                //up to here we know this guy being assigned role has a maximum of contractor role in other company. No default of admin. 
                //just let pass the check,   mark him/her as default for this company, delete all property allocation for this company.
            }
            return string.Empty;
        }

        public IQueryable<MemberInfo> GetMemberInfoOutsideCompany(int companyId, int memberId)
        {
            // get all the memberInfo 


            var members = from com in _ctx.Companies
                          join cm in _ctx.CompanyMembers on com.Id equals cm.CompanyId
                          join mem in _ctx.Members on cm.MemberId equals mem.Id
                          join user in _ctx.Users on mem equals user.Member
                          where com.Id != companyId && mem.Id == memberId
                          select new MemberInfo
                          {
                              Member = mem,//member record
                              CompanyMember = cm,//join record
                              Company = com,
                              User = user,
                          };

            return members;
        }


        /// <summary>
        /// this method just do the update, validation is passed before
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="memberId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public CompanyRole DoUpdateCompanyMemberRole(int companyId, int memberId, CompanyRole role)
        {
           // var companyId = GetCompanyFoAdminUser(userName).Id;
            var memberInfo = GetMemberByCompanyIdQuery(companyId, memberId);
            if (memberInfo.Count() > 1)
            {
                throw new Exception("Multiple member found with ID" + memberId);
            }
            var cmRecord = _ctx.CompanyMembers.Where(p => p.CompanyId == companyId && p.MemberId == memberId).ToList().FirstOrDefault();

            if (role == CompanyRole.Default)
            {
                //up have passed all validation so here we can do the update.
                //here we directly mark this guy as contractor in all other companies. 
                //once we have the request function we need send a request here, then on accepting the request we perform the action
                _ctx.CompanyMembers.Where(p => p.CompanyId != companyId && p.MemberId == memberId).Update(p => new CompanyMember()
                {
                    Role = CompanyRole.Contractor,
                    ModifiedDateTime = DateTime.Now,
                });

                //delete all allocation
                _ctx.PropertyAllocations.RemoveRange(cmRecord.PropertyAllocations);
            }
            cmRecord.Role = role;


            _ctx.Entry(cmRecord).State = EntityState.Modified;

            _ctx.SaveChanges();
            return role;

        }


        // for now when member join company we give contractor role by default
        public void DoMemberJoinCompany(int companyId, int memberId)
        {
            // var companyId = GetCompanyFoAdminUser(userName).Id;
            var memberInfo = GetMemberByCompanyIdQuery(companyId, memberId);
            if (memberInfo.Count() > 1)
            {
                throw new Exception("Multiple member found with ID" + memberId);
            }
            var cmRecord = _ctx.CompanyMembers.Where(p => p.CompanyId == companyId && p.MemberId == memberId).ToList().FirstOrDefault();

            if(cmRecord != null)
            {
                throw new Exception("Member already exists in company");
            }

            var newCmRecord = new CompanyMember()
            {
                CompanyId = companyId,
                MemberId = memberId,
                Role = CompanyRole.Contractor,//by default contractor for now. We can pass in role for join company later.
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                Confirmed = true

            };
            _ctx.Entry(newCmRecord).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        public void DoClientAddToCompany(int companyId, int clientId)
        {
            var propertiesForClient =
                _ctx.ClientProperties.Where(p => p.ClientId == clientId).Select(p => p.PropertyId).ToList();
            foreach (var propertyId in propertiesForClient)
            {
                if (_ctx.PropertyCompanies.Any(p => p.CompanyId == companyId && p.PropertyId == propertyId)) continue;
                var newCpRecord = new PropertyCompany()
                {
                    CompanyId = companyId,
                    PropertyId = propertyId,
                    AddedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                       
                };
                _ctx.Entry(newCpRecord).State = EntityState.Added;
            }
            _ctx.SaveChanges();
        }


        public Company GetCompanyFoAdminUser(string userName)
        {
            return GetCompanyForUser(userName, new List<CompanyRole> { CompanyRole.Admin });
        } 
        public Company GetCompanyForUser(string userName)
        {
            return GetCompanyForUser(userName ,new List<CompanyRole> { CompanyRole.Default, CompanyRole.Admin});
        }
        public string GetCompanyLogoUrl(int companyId)
        {
            var logo =  _ctx.Attchments.OrderByDescending(p=> p.AddedDateTime).FirstOrDefault(p => p.EntityType == AttachmentEntityType.CompanyLogo && p.EntityId == companyId);
            return logo?.Url;
        }

        private Company GetCompanyForUser(string userName, List<CompanyRole> roles)
        {
            //user must be admin.
            var user = _authRepo.GetUserByUserName(userName);

            if (user.UserType != UserType.Trade)
                throw new Exception("Only member can view company members");


            _ctx.Entry(user).Reference(s => s.Member).Load();

            if (user.Member == null)
                throw new Exception("Only member can view company members");

            _ctx.Entry(user.Member).Collection(s => s.CompanyMembers).Load();
            var company = user.Member.CompanyMembers.ToList().FirstOrDefault(r => roles.Contains(r.Role));

            if (company == null)
                throw new Exception("Cannot find company for user");


            _ctx.Entry(company).Reference(s => s.Company).Load();
            // var result = _ctx.Companies.Find(company.CompanyId);
            _ctx.Entry(company.Company).Collection(s => s.CompanyServices).Load();
            return company.Company;
        }

        private IQueryable<MemberInfo> GetMemberByCompanyIdQuery(int companyId, int? memberId = null)
        {
            var members = from com in _ctx.Companies
                          join cm in _ctx.CompanyMembers on com.Id equals cm.CompanyId
                          join mem in _ctx.Members on cm.MemberId equals mem.Id
                          join user in _ctx.Users on mem equals user.Member
                          where com.Id == companyId && (!memberId.HasValue || mem.Id == memberId)
                          select new MemberInfo
                          {
                              Member = mem,//member record
                              CompanyMember = cm,//join record
                              Company = com,
                              User = user,

                          };

            return members;
        }



        public ApplicationUser GetCompanyAdminMember(int companyId)
        {
            var user = from com in _ctx.Companies
                          join cm in _ctx.CompanyMembers on com.Id equals cm.CompanyId
                          join mem in _ctx.Members on cm.MemberId equals mem.Id
                          join u in _ctx.Users on mem equals u.Member
                          where com.Id == companyId && cm.Role== CompanyRole.Admin
                          select u;

            return user.First();
        }


        public IQueryable<MemberSearchModel> SearchMemberForJoinCompany(string userName, string searchText)
        {
            var search = searchText.ToLower();
            var companyId = GetCompanyFoAdminUser(userName).Id;
            var result = ( from mem in _ctx.Members 
                          where mem.CompanyMembers.All(p => p.CompanyId != companyId) //not in the company
                           && mem.CompanyMembers.All(p => p.Role != CompanyRole.Admin)  //cannot be Admin in any other company
                           && (mem.FirstName.ToLower().Contains(search)
                           || mem.LastName.ToLower().Contains(search)
                           || mem.Email.ToLower().Contains(search))//search
                           select new MemberSearchModel
                                     {
                                         FullName = mem.FirstName + " " + mem.LastName,
                                         Email = mem.Email,
                                         MemberId = mem.Id,
                                     }).Distinct().Take(10);// search result get maximum 10.             
            return result;
        }

        public IQueryable<ClientSearchModel> SearchClientForCompanyInvite(string userName, string searchText)
        {
            var search = searchText.ToLower();
            var companyId = GetCompanyForUser(userName).Id;
            var alreadyClientIds = (
                from client in _ctx.Clients
                join cp in _ctx.ClientProperties on client.Id equals cp.ClientId
                join pc in _ctx.PropertyCompanies on cp.PropertyId equals pc.PropertyId
                where pc.CompanyId == companyId

                select client.Id
            );
            var result = (from client in _ctx.Clients
                //no property for that company
                where (client.FirstName.ToLower().Contains(search)
                       || client.LastName.ToLower().Contains(search)
                       || client.Email.ToLower().Contains(search)) && !alreadyClientIds.Contains(client.Id)

                select new ClientSearchModel
                {
                    FullName = client.FirstName + " " + client.LastName,
                    Email = client.Email,
                    ClientId = client.Id,
                }).Distinct().Take(10); // search result get maximum 10.             
            return result;
        }


        public void UpdateMemberServiceTypes(string userName, int memberId, List<TradeType> types)
        {
            var companyId = GetCompanyFoAdminUser(userName).Id;
            var record = _ctx.CompanyMembers.Single(p => p.CompanyId == companyId && p.MemberId == memberId);
            if(record.Role == CompanyRole.Admin)
            {
                throw new Exception("Permission for Admin cannot be modified");
            }
            record.AllowedTradeTypes = types;
            _ctx.Entry(record).State = EntityState.Modified;
            _ctx.SaveChanges();
        }
        public void GenerateAddClientToCompany(int clientId, int companyId, string messageFromRequestor)
        {
            var companyName = _ctx.Companies.Find(companyId)?.Name;
            var clientName = _ctx.Clients.Find(clientId)?.FirstName;

            var memberUser = _ctx.Users.FirstOrDefault(p => p.Client.Id == clientId);

            var adminUser = GetCompanyAdminMember(companyId);

            var systemMessage = string.Format(InviteClientJoinCompanyRequestMessage, clientName, companyName);
            // var messageToSend = systemMessage + char(13) + CHAR(10) + messageFromRequestor;


            var sb = new StringBuilder();
            sb.AppendLine(systemMessage);
            sb.AppendLine("<br/>");
            sb.AppendLine(messageFromRequestor);
            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                ClientId = clientId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = sb.ToString(),
                MessageType = MessageType.InviteClientToCompany,
                IsWaitingForResponse = true,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }

        public void GenerateAddMemberToCompany(int memberId, int companyId, string messageFromRequestor, CompanyRole role)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;

            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = GetCompanyAdminMember(companyId);

            var systemMessage = string.Format(InviteJoinCompanyRequestMessage, memberName, companyName);
            // var messageToSend = systemMessage + char(13) + CHAR(10) + messageFromRequestor;


            var sb = new StringBuilder();
            sb.AppendLine(systemMessage);
            sb.AppendLine("<br/>");
            sb.AppendLine(messageFromRequestor);
            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                Role = role,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = sb.ToString(),
                MessageType = MessageType.InviteJoinCompanyRequest,
                IsWaitingForResponse = true,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }


        public void GenerateAssignDefaultRoleMessage(int memberId, int companyId)
        {
            var companyName = _ctx.Companies.Find(companyId).Name;
            var member = _ctx.Members.Find(memberId);
            var memberName = member.FirstName;
            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = GetCompanyAdminMember(companyId);

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = string.Format(AssignDefaultRoleMessage, memberName, companyName),
                MessageType = MessageType.AssignDefaultRole,
                IsWaitingForResponse = false,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            //_ctx.SaveChanges();
        }

        public void GenerateAssignDefaultRoleRequestMessage(int memberId, int companyId)
        {


            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;

            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = GetCompanyAdminMember(companyId);

            var otherCompanyNames = GetMemberInfoOutsideCompany(companyId, memberId)
                .Where(p => p.CompanyMember.Role == CompanyRole.Default)
                .Select(p => p.Company)
                .ToList()
                .Select(p => p.Name)
                .Aggregate((x, y) => x + ", " + y);
            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = string.Format(AssignDefaultRoleRequestMessage, memberName, companyName, otherCompanyNames),
                MessageType = MessageType.AssignDefaultRoleRequest,
                IsWaitingForResponse = true,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            _ctx.SaveChanges();
        }



        public bool CheckIfThereIsWaitingDefaultRoleRequestMessage(int memberId, int companyId)
        {
            var any = _ctx.Messages.Any(p => p.MemberId == memberId
            && p.CompanyId == companyId
            && p.MessageType == MessageType.AssignDefaultRoleRequest
            && p.IsWaitingForResponse == true);
            return any;

        }

        public void GenerateAssignContractorRoleMessage(int memberId, int companyId)
        {
            var memberUser = _ctx.Users.First(p => p.Member.Id == memberId);

            var adminUser = GetCompanyAdminMember(companyId);
            var companyName = _ctx.Companies.Find(companyId).Name;
            var memberName = _ctx.Members.Find(memberId).FirstName;

            var message = new Message()
            {
                AddedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CompanyId = companyId,
                MemberId = memberId,
                UserIdFrom = adminUser.Id,
                UserIdTo = memberUser.Id,
                MessageText = string.Format(AssignContractorRoleMessage, memberName, companyName),
                MessageType = MessageType.AssignContractorRole,
                IsWaitingForResponse = false,
                IsRead = false,
            };

            _ctx.Entry<Message>(message).State = EntityState.Added;
            //_ctx.SaveChanges();
        }






    }
}
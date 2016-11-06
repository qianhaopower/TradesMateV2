
using DataService.Entities;
using DataService.Infrastructure;
using DataService.Models;
using EntityFramework.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace EF.Data
{

    public class CompanyRepository : IDisposable
    {
        private EFDbContext _ctx;

        private UserManager<ApplicationUser> _userManager;
        public CompanyRepository(EFDbContext ctx = null)
        {
            if(ctx!= null)
            {
                _ctx = ctx;
            }
            else
            {
                _ctx = new EFDbContext();
            }
           
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
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
            

            new MessageRepository(_ctx).GenerateAddMemberToCompany(model.MemberId, companyId, model.Text, CompanyRole.Contractor);//for now by default contractor.
        }

        public IQueryable<MemberModel> GetMemberByUserName(string userName, int? memberId = null)
        {
            var companyId = GetCompanyFoAdminUser(userName).Id;
            var result = GetMemberByCompanyIdQuery(companyId, memberId);
            return result.Select(p => new MemberModel
            {
                FirstName = p.Member.FirstName,
                LastName = p.Member.LastName,
                Email = p.Member.Email,
                MemberRole = p.CompanyMember.Role.ToString(),
                MemberId = p.Member.Id,
                Username = p.User.UserName,

            });

        }


        //public Company GetCompanyForCurrentUser(string userName)
        //{
        //    var company = GetCompanyFoAdminUser(userName);
        //    return company;

        //}

        public IQueryable<Property> GetCompanyProperties(int companyId)
        {
            // get the company that this property has been assigned to.
            IQueryable<Property> properties = _ctx.Companies.Where(p => p.Id == companyId).SelectMany(p => p.PropertyCompanies).Select(p => p.Property);
            return properties;


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
            var _repo = new AuthRepository(_ctx);
            var isUserAdminTask = await _repo.isUserAdminAsync(userName);

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
                var repo = new MessageRepository(_ctx);
                var companyId = GetCompanyFoAdminUser(userName).Id;
                CompanyRole roleParsed;
                bool roleValid = Enum.TryParse<CompanyRole>(role, out roleParsed);
                switch (messageType)
                {
                    //here we generate message to user. 
                    case MessageType.AssignDefaultRole:
                        repo.GenerateAssignDefaultRoleMessage(memberId, companyId);
                        //let it happen, no need to wait
                        DoUpdateCompanyMemberRole( companyId,memberId, roleParsed);
                        break;
                    case MessageType.AssignContractorRole:
                        repo.GenerateAssignContractorRoleMessage(memberId, companyId);
                        DoUpdateCompanyMemberRole( companyId, memberId, roleParsed);
                        //let it happen, no need to wait
                        break;
                    case MessageType.AssignDefaultRoleRequest:// need wait for the request's response
                        repo.GenerateAssignDefaultRoleRequestMessage(memberId, companyId);
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
            var _repo = new AuthRepository(_ctx);
            var isUserAdminTask =  _repo.isUserAdmin(userName);

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
                    var repo = new MessageRepository(_ctx);
                    if (repo.CheckIfThereIsWaitingDefaultRoleRequestMessage(memberId, companyId))
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

        public Company GetCompanyFoAdminUser(string userName)
        {
            var _repo = new AuthRepository(_ctx);

            //user must be admin.
            var user = _repo.GetUserByUserName(userName);

            if (user.UserType != UserType.Trade)
                throw new Exception("Only member can view company members");


            _ctx.Entry(user).Reference(s => s.Member).Load();

            if (user.Member == null)
                throw new Exception("Only member can view company members");

            _ctx.Entry(user.Member).Collection(s => s.CompanyMembers).Load();
            var company = user.Member.CompanyMembers.ToList().FirstOrDefault(p => p.Role == CompanyRole.Admin);

            if (company == null)
                throw new Exception("Only admin member can view company members");


            _ctx.Entry(company).Reference(s => s.Company).Load();
            return company.Company;
        }

        private IQueryable<MemberInfo> GetMemberByCompanyIdQuery(int companyId, int? memberId = null)
        {


            var members = from com in _ctx.Companies
                          join cm in _ctx.CompanyMembers on com.Id equals cm.CompanyId
                          join mem in _ctx.Members on cm.MemberId equals mem.Id
                          join user in _ctx.Users on mem equals user.Member
                          where com.Id == companyId && (memberId.HasValue ? mem.Id == memberId : true)
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
            //var result = 
            //              (from cm in _ctx.CompanyMembers 
            //              join mem in _ctx.Members on cm.MemberId equals mem.Id
            //              where cm.CompanyId != companyId //not in the company
            //              && cm.Role != CompanyRole.Admin //cannot be Admin in any other company
            //              &&(mem.FirstName.ToLower().Contains(search)
            //              || mem.LastName.ToLower().Contains(search)
            //              || mem.Email.ToLower().Contains(search))//search
            //              select new MemberSearchModel
            //              {
            //                  FullName = mem.FirstName + " "+ mem.LastName ,
            //                   Email = mem.Email,
            //                    MemberId = mem.Id,
            //              }).Distinct().Take(10);// search result get maximum 10. 

            var result = ( from mem in _ctx.Members 
                          where !mem.CompanyMembers.Any(p => p.CompanyId == companyId) //not in the company
                           && !mem.CompanyMembers.Any(p => p.Role == CompanyRole.Admin)  //cannot be Admin in any other company
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
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }

    }
}
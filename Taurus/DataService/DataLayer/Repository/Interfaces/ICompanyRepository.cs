
using DataService.Infrastructure;
using DataService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace EF.Data
{

    public interface ICompanyRepository : IBaseRepository
    {
        IQueryable<Company> GetAllCompanies();
        void CreateJoinCompanyRequest(string userName, InviteMemberModel model);
        IEnumerable<MemberModel> GetMemberByUserName(string userName, int? memberId = null);
        IQueryable<Property> GetCompanyProperties(int companyId);
        void UpdateCompany(CompanyModel companyModel);
        Task RemoveMemberFromCompnay(string userName, int memberId);
        MessageType? UpdateCompanyMemberRole(string userName, int memberId, string role);
        IQueryable<MemberInfo> GetMemberInfoOutsideCompany(int companyId, int memberId);
        CompanyRole DoUpdateCompanyMemberRole(int companyId, int memberId, CompanyRole role);
        void DoMemberJoinCompany(int companyId, int memberId);
        Company GetCompanyFoAdminUser(string userName);
        Company GetCompanyForUser(string userName);
        ApplicationUser GetCompanyAdminMember(int companyId);
        IQueryable<MemberSearchModel> SearchMemberForJoinCompany(string userName, string searchText);
        IQueryable<ClientSearchModel> SearchClientForCompanyInvite(string userName, string searchText);
        void UpdateMemberServiceTypes(string userName, int memberId, List<TradeType> types);
        string GetCompanyLogoUrl(int companyId);
        void CreateInviteToCompanyRequest(string identityName, InviteClientModel model);

        void DoClientAddToCompany(int companyId, int clientId);
    }
}
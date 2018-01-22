﻿using System.Linq;
using System.Net;
using System.Web.Http;
using EF.Data;
using System.Threading.Tasks;
using System.Web.Http.Description;
using DataService.Models;
using AutoMapper;

namespace DataService.Controllers
{
    [Authorize]
    [RoutePrefix("api/companies")]
    public class CompaniesController : ApiController
    {

        private IAuthRepository _authRepo;
        private readonly ICompanyRepository _companyRepo;
        public CompaniesController(IAuthRepository authRepo, ICompanyRepository companyRepo)
        {
            _authRepo = authRepo;
            _companyRepo = companyRepo;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCompanyForCurrentUser()
        {
            var company = _companyRepo.GetCompanyFoAdminUser(User.Identity.Name);
            return Ok(Mapper.Map<Company, CompanyModel>(company));
        }
        [HttpGet]
        [Route("all")]
        public IHttpActionResult GetCompanies()
        {
            var result = _companyRepo.GetAllCompanies()
             .Select(Mapper.Map<Company, CompanyModel>).ToList();
            //no need for the credit card field
            result.ForEach(p => p.CreditCard = null);
            return Ok(result);
        }



        [HttpGet]
        [Route("member")]
        public  IHttpActionResult GetCurrentCompanyMembers()
        {

            //get the current user's company members

                // the user must be of type trades, also the user need to be Admin. The check is in GetMemberByUserName
                var memberList = _companyRepo.GetMemberByUserName(User.Identity.Name);

               // var modelList = memberList.ToList().Select(Mapper.Map<Member, MemberModel>);

                return (Ok(memberList));

        }

        [HttpGet]
        [Route("member/search")]
        public IHttpActionResult SearchMemberForJoinCompany(string searchText)
        {
            var searchList = _companyRepo.SearchMemberForJoinCompany(User.Identity.Name, searchText);

            return (Ok(searchList));
        }



        [HttpPut]
        [Route("member/{id:int}")]
        public   IHttpActionResult UpdateCompanyMemberRole(int id, string role)
        {
            var messageType = _companyRepo.UpdateCompanyMemberRole(User.Identity.Name, id, role);

            string result = messageType == MessageType.AssignDefaultRoleRequest ?
                "Member has default role in other company, we have send a request to him/her." : string.Empty;  
            return (Ok(result));

        }


        [HttpPost]
        [Route("member/add")]
        public IHttpActionResult AddExistingMemberToCompany(InviteMemberModel model)
        {

            //todo 1) add text in to the message,
            //2) validate email not exist. 
            //3) validate this member is not admin for any other company. 
            _companyRepo.CreateJoinCompanyRequest(User.Identity.Name, model);
            return (Ok());

        }

        [HttpPost]
        [Route("memeber/update")]
        public IHttpActionResult UpdateMemberServiceTypes(UpdateMemberServiceTypeModel model)
        {
            _companyRepo.UpdateMemberServiceTypes(User.Identity.Name, model.MemberId,model.SelectedTypes);
            return (Ok());
        }

        [HttpDelete]
        [Route("member/{id:int}")]
        public async Task<IHttpActionResult> RemoveMember(int id)
        {
            await _companyRepo.RemoveMemberFromCompnay(User.Identity.Name, id);
            return (Ok());

        }

        [HttpGet]
        [Route("member/{id:int}")]
        public IHttpActionResult GetCurrentCompanyMember(int id)
        {
           

            //get the current user's company members

            // the user must be of type trades, also the user need to be Admin. The check is in GetMemberByUserName
            var member = _companyRepo.GetMemberByUserName(User.Identity.Name, id).First();

            // var modelList = memberList.ToList().Select(Mapper.Map<Member, MemberModel>);
            return Ok(member);
           

        }
       
        [HttpPut]
        [ResponseType(typeof(void))]
        [Route("")]
        public IHttpActionResult ModifyCompany( CompanyModel companyModel)
        {
            _companyRepo.UpdateCompany(companyModel);
            return StatusCode(HttpStatusCode.NoContent);
        }
       
    }
}

using System.Linq;
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
    public class CompaniesController : ApiController
    {

        private IAuthRepository _authRepo;
        private ICompanyRepository _companyRepo;
        public CompaniesController(IAuthRepository authRepo, ICompanyRepository companyRepo)
        {
            _authRepo = authRepo;
            _companyRepo = companyRepo;
        }

        public IHttpActionResult GetCompanyForCurrentUser()
        {
            var company = _companyRepo.GetCompanyFoAdminUser(User.Identity.Name);
            return Ok(Mapper.Map<Company, CompanyModel>(company));
        }


        public  IHttpActionResult GetCurrentCompanyMembers()
        {

            //get the current user's company members

                // the user must be of type trades, also the user need to be Admin. The check is in GetMemberByUserName
                var memberList = _companyRepo.GetMemberByUserName(User.Identity.Name);

               // var modelList = memberList.ToList().Select(Mapper.Map<Member, MemberModel>);

                return (Ok(memberList));

        }

        [HttpGet]
        public IHttpActionResult SearchMemberForJoinCompany(string searchText)
        {
            var searchList = _companyRepo.SearchMemberForJoinCompany(User.Identity.Name, searchText);

            return (Ok(searchList));
        }



        [HttpPost]
        public   IHttpActionResult UpdateCompanyMemberRole(int memberId, string role)
        {
            var messageType = _companyRepo.UpdateCompanyMemberRole(User.Identity.Name, memberId, role);

            string result = messageType == MessageType.AssignDefaultRoleRequest ?
                "Member has default role in other company, we have send a request to him/her." : string.Empty;  
            return (Ok(result));

        }


        [HttpPost]
        public IHttpActionResult AddExistingMemberToCompany(InviteMemberModel model)
        {

            //todo 1) add text in to the message,
            //2) validate email not exist. 
            //3) validate this member is not admin for any other company. 
            _companyRepo.CreateJoinCompanyRequest(User.Identity.Name, model);
            return (Ok());

        }

        [HttpPost]
        public IHttpActionResult UpdateMemberServiceTypes(UpdateMemberServiceTypeModel model)
        {
            _companyRepo.UpdateMemberServiceTypes(User.Identity.Name, model.MemberId,model.SelectedTypes);
            return (Ok());
        }

        [HttpDelete]
        public async Task<IHttpActionResult> RemoveMember(int memberId )
        {
            await _companyRepo.RemoveMemberFromCompnay(User.Identity.Name, memberId);
            return (Ok());

        }

        [HttpGet]
        public IHttpActionResult GetCurrentCompanyMember(int memberId)
        {
           

            //get the current user's company members

            // the user must be of type trades, also the user need to be Admin. The check is in GetMemberByUserName
            var member = _companyRepo.GetMemberByUserName(User.Identity.Name, memberId).First();

            // var modelList = memberList.ToList().Select(Mapper.Map<Member, MemberModel>);
            return Ok(member);
           

        }
       
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult ModifyCompany( CompanyModel companyModel)
        {
            _companyRepo.UpdateCompany(companyModel);
            return StatusCode(HttpStatusCode.NoContent);
        }
       
    }
}

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
        //private EFDbContext db = new EFDbContext();

        public IHttpActionResult GetCompanyForCurrentUser()
        {
            var company = new CompanyRepository().GetCompanyFoAdminUser(User.Identity.Name);
            return Ok(Mapper.Map<Company, CompanyModel>(company));
        }


        public  IHttpActionResult GetCurrentCompanyMembers()
        {

            //get the current user's company members

                // the user must be of type trades, also the user need to be Admin. The check is in GetMemberByUserName
                var memberList =  new CompanyRepository().GetMemberByUserName(User.Identity.Name);

               // var modelList = memberList.ToList().Select(Mapper.Map<Member, MemberModel>);

                return (Ok(memberList));

        }

        [HttpGet]
        public IHttpActionResult SearchMemberForJoinCompany(string searchText)
        {
            var searchList = new CompanyRepository().SearchMemberForJoinCompany(User.Identity.Name, searchText);

            return (Ok(searchList));
        }



        [HttpPost]
        public   IHttpActionResult UpdateCompanyMemberRole(int memberId, string role)
        {
            var messageType = new CompanyRepository().UpdateCompanyMemberRole(User.Identity.Name, memberId, role);

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
             new CompanyRepository().CreateJoinCompanyRequest(User.Identity.Name, model);
            return (Ok());

        }

        [HttpPost]
        public IHttpActionResult UpdateMemberServiceTypes(UpdateMemberServiceTypeModel model)
        {
            new CompanyRepository().UpdateMemberServiceTypes(User.Identity.Name, model.MemberId,model.SelectedTypes);
            return (Ok());
        }

        [HttpDelete]
        public async Task<IHttpActionResult> RemoveMember(int memberId )
        {
            await new CompanyRepository().RemoveMemberFromCompnay(User.Identity.Name, memberId);
            return (Ok());

        }

        [HttpGet]
        public IHttpActionResult GetCurrentCompanyMember(int memberId)
        {
            var _repo = new AuthRepository();

            //get the current user's company members

            // the user must be of type trades, also the user need to be Admin. The check is in GetMemberByUserName
            var member = new CompanyRepository().GetMemberByUserName(User.Identity.Name, memberId).First();

            // var modelList = memberList.ToList().Select(Mapper.Map<Member, MemberModel>);
            return Ok(member);
           

        }
       
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult ModifyCompany( CompanyModel companyModel)
        {
            new CompanyRepository().UpdateCompany(companyModel);
            return StatusCode(HttpStatusCode.NoContent);
        }
       
    }
}

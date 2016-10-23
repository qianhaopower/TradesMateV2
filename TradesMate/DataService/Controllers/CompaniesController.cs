using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using EF.Data;
using DataService.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http.Description;
using DataService.Models;
using AutoMapper;

namespace DataService.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using EF.Data;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Company>("Companies");
    builder.EntitySet<Address>("Addresses"); 
    builder.EntitySet<WorkItemTemplate>("WorkItemTemplates"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CompaniesOdataController : ODataController
    {
        private EFDbContext db = new EFDbContext();

        // GET: odata/Companies
        [EnableQuery]
        public IQueryable<Company> GetCompanies()
        {
            return db.Companies;
        }

        // GET: odata/Companies(5)
        [EnableQuery]
        public SingleResult<Company> GetCompany([FromODataUri] int key)
        {
            return SingleResult.Create(db.Companies.Where(company => company.Id == key));
        }
       



        // PUT: odata/Companies(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Company> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Company company = db.Companies.Find(key);
            if (company == null)
            {
                return NotFound();
            }

            patch.Put(company);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(company);
        }

        // POST: odata/Companies
        public IHttpActionResult Post(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Companies.Add(company);
            db.SaveChanges();

            return Created(company);
        }

        // PATCH: odata/Companies(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Company> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Company company = db.Companies.Find(key);
            if (company == null)
            {
                return NotFound();
            }

            patch.Patch(company);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(company);
        }

        // DELETE: odata/Companies(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Company company = db.Companies.Find(key);
            if (company == null)
            {
                return NotFound();
            }

            db.Companies.Remove(company);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Companies(5)/Address
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.Companies.Where(m => m.Id == key).Select(m => m.Address));
        }

        // GET: odata/Companies(5)/WorkItemTemplateList
        [EnableQuery]
        public IQueryable<WorkItemTemplate> GetWorkItemTemplateList([FromODataUri] int key)
        {
            return db.Companies.Where(m => m.Id == key).SelectMany(m => m.WorkItemTemplateList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(int key)
        {
            return db.Companies.Count(e => e.Id == key) > 0;
        }
    }


    public class CompaniesController : ApiController
    {
        private EFDbContext db = new EFDbContext();

        public IHttpActionResult GetCompanyForCurrentUser()
        {
            var company = new CompanyRepository().GetCompanyForCurrentUser(User.Identity.Name);
            return Ok(Mapper.Map<Company, CompanyModel>(company));

        }


        public  IHttpActionResult GetCurrentCompanyMembers()
        {
            var _repo = new AuthRepository();

            //get the current user's company members

                // the user must be of type trades, also the user need to be Admin. The check is in GetMemberByUserName
                var memberList =  new CompanyRepository().GetMemberByUserName(User.Identity.Name);

               // var modelList = memberList.ToList().Select(Mapper.Map<Member, MemberModel>);

                return (Ok(memberList));

        }

        [HttpPost]
        public async  Task<IHttpActionResult> UpdateCompanyMemberRole(int memberId, string role)
        {
            var newRole = await new CompanyRepository().UpdateCompanyMemberRole(User.Identity.Name, memberId, role);
               
            return (Ok(newRole.ToString()));

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


        // GET: api/Companies
        public IQueryable<Company> GetCompanies()
        {
            return db.Companies;
        }

        // GET: api/Companies/5
        [ResponseType(typeof(Company))]
        public IHttpActionResult GetCompany(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult ModifyCompany( CompanyModel companyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var company = db.Companies.First(p => p.Id == companyModel.CompanyId);

            if (company == null)
            {
                return BadRequest("Cannot fine company");
            }
            company.Description = companyModel.Description;
            company.Name = companyModel.CompanyName;
            company.CreditCard = companyModel.CreditCard;

            db.Entry(company).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(companyModel.CompanyId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Companies
        [ResponseType(typeof(Company))]
        public IHttpActionResult PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Companies.Add(company);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = company.Id }, company);
        }

        // DELETE: api/Companies/5
        [ResponseType(typeof(Company))]
        public IHttpActionResult DeleteCompany(int id)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }

            db.Companies.Remove(company);
            db.SaveChanges();

            return Ok(company);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(int id)
        {
            return db.Companies.Count(e => e.Id == id) > 0;
        }
    }
}

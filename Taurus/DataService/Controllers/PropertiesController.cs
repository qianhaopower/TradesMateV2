﻿using System;
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
using AutoMapper;
using DataService.Models;
using DataService.Infrastructure;

namespace DataService.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using EF.Data;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Property>("Properties");
    builder.EntitySet<Address>("Addresses"); 
    builder.EntitySet<Client>("Clients"); 
    builder.EntitySet<Section>("Sections"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [Authorize]
    public class PropertiesController : ODataController
    {
        private EFDbContext db = new EFDbContext();

        // GET: odata/Properties
        [EnableQuery]
        [HttpGet]
        public IQueryable<Property> GetProperties()
        {
            var repo = new PropertyRepository();

            var properties = repo.GetPropertyForUser(User.Identity.Name).Include(p => p.Address);

            // bing the property model back when we decide to use normal model or OData EDM model.
            //var results= properties.Select(Mapper.Map<Property, PropertyModel>).ToList();
            return properties;


            // return db.Properties;
        }

        //[HttpGet]
        //http://localhost/DataService/odata/GetPropertyCompanies(propertyId=1)
        //[ODataRoute("GetPropertyCompanies(propertyId={propertyId})")]
        //public List<CompanyModel> GetPropertyCompanies([FromODataUri]int propertyId)
        //{
        //    var repo = new PropertyRepository();
        //    var companyModels = repo.GetCompanyForProperty(propertyId).Select( Mapper.Map<Company, CompanyModel>).ToList();
        //    //no need for the credit card field
        //    companyModels.ForEach(p => p.CreditCard = null);
        //    return companyModels;
        //}

        [HttpGet]
        //http://localhost/DataService/odata/GetMemberAllocation(memberId=1)
        [ODataRoute("GetMemberAllocation(memberId={memberId})")]
        public IHttpActionResult GetMemberAllocation(int memberId)
        {
            var memberList = new PropertyRepository().GetMemberAllocation(User.Identity.Name, memberId).ToList();
            return (Ok(memberList));

        }


        [HttpPost]
        //http://localhost/DataService/odata/UpdateMemberAllocation(memberId=1)
        [ODataRoute("UpdateMemberAllocation(propertyId={propertyId},memberId={memberId},allocated={allocate})")]
        public IHttpActionResult UpdateMemberAllocation(int propertyId, int memberId, bool allocate)
        {
            var allcation = new PropertyRepository().UpdateMemberAllocation(User.Identity.Name, propertyId, memberId, allocate);
            return (Ok(allcation));

        }

        [HttpGet]
        //  http://localhost/DataService/odata/Properties(1)/SomeFunction
        public IHttpActionResult SomeFunction()
        {
            return Ok("Some");
        }
        //  http://localhost/DataService/odata/Properties(1)/GetCompanies
        [HttpGet]
        public IHttpActionResult GetCompanies([FromODataUri]  int key)
        {
            var repo = new PropertyRepository();
            var companyModels = repo.GetCompanyForProperty(key).Select(Mapper.Map<Company, CompanyModel>).ToList();
            //no need for the credit card field
            companyModels.ForEach(p => p.CreditCard = null);
            return Ok(companyModels);
        }


        // GET: odata/Properties(5)
        [EnableQuery]
        public Property GetProperty([FromODataUri] int key)
        {
            var repo = new PropertyRepository();

            var property = repo.GetPropertyForUser(User.Identity.Name).Include(p => p.Address).FirstOrDefault(p => p.Id == key);
            //var result = Mapper.Map<Property, PropertyModel>(property);
            //return SingleResult.Create(property);
            return property;
        }

        // PUT: odata/Properties(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Property> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Property property = db.Properties.Find(key);
            if (property == null)
            {
                return NotFound();
            }

            patch.Put(property);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(property);
        }



        // PATCH: odata/Properties(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Property> patch)
        {
            //Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Property property = db.Properties.Find(key);
            if (property == null)
            {
                return NotFound();
            }
            patch.Patch(property);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(property);
        }

        // DELETE: odata/Properties(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Property property = db.Properties.Find(key);
            if (property == null)
            {
                return NotFound();
            }

            db.Properties.Remove(property);
            db.ClientProperties.RemoveRange(db.ClientProperties.Where(p=> p.PropertyId == key).ToList());
            db.PropertyCompanies.RemoveRange(db.PropertyCompanies.Where(p => p.PropertyId == key).ToList());
            db.PropertyAllocations.RemoveRange(db.PropertyAllocations.Where(p => p.PropertyId == key).ToList());
            db.Sections.RemoveRange(db.Sections.Where(p => p.PropertyId == key).ToList());
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Properties(5)/Address
        [EnableQuery]
        public SingleResult<Address> GetAddress([FromODataUri] int key)
        {
            return SingleResult.Create(db.Properties.Where(m => m.Id == key).Select(m => m.Address));
        }

        // GET: odata/Properties(5)/Client
        [EnableQuery]
        public SingleResult<Client> GetClient([FromODataUri] int key)
        {
            //By defualt get the owner client, display the owner client name
            var ownerClient = new PropertyRepository().GetPropertyOwnerClinet(key);
            return SingleResult.Create(ownerClient);
        }

        // GET: odata/Properties(5)/SectionList
        [EnableQuery]
        public IQueryable<Section> GetSectionList([FromODataUri] int key)
        {
            return db.Properties.Where(m => m.Id == key).SelectMany(m => m.SectionList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PropertyExists(int key)
        {
            return db.Properties.Count(e => e.Id == key) > 0;
        }
    }




    [Authorize]
    public class PropertiesWebApiController : ApiController
    {


        [HttpPost]
        public IHttpActionResult CreatePropertyForClient(PropertyModel model)
        {
            var property = new PropertyRepository().CreatePropertyForClient(User.Identity.Name, model);

            return Ok();

        }
    }

}

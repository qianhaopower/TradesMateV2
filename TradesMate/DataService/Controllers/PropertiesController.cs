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
    public class PropertiesController : ODataController
    {
        private EFDbContext db = new EFDbContext();

        // GET: odata/Properties
        [EnableQuery]
        public IQueryable<Property> GetProperties()
        {
            return db.Properties;
        }

        // GET: odata/Properties(5)
        [EnableQuery]
        public SingleResult<Property> GetProperty([FromODataUri] int key)
        {
            return SingleResult.Create(db.Properties.Where(property => property.Id == key));
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

        // POST: odata/Properties
        public IHttpActionResult Post(Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Properties.Add(property);
            db.SaveChanges();

            return Created(property);
        }

        // PATCH: odata/Properties(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Property> patch)
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
            return SingleResult.Create(db.Properties.Where(m => m.Id == key).Select(m => m.Client));
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
}

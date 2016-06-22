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
    builder.EntitySet<Section>("Sections");
    builder.EntitySet<Property>("Properties"); 
    builder.EntitySet<WorkItem>("WorkItems"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SectionsController : ODataController
    {
        private EFDbContext db = new EFDbContext();

        // GET: odata/Sections
        [EnableQuery]
        public IQueryable<Section> GetSections()
        {
            return db.Sections;
        }

        // GET: odata/Sections(5)
        [EnableQuery]
        public SingleResult<Section> GetSection([FromODataUri] int key)
        {
            return SingleResult.Create(db.Sections.Where(section => section.Id == key));
        }

        // PUT: odata/Sections(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Section> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Section section = db.Sections.Find(key);
            if (section == null)
            {
                return NotFound();
            }

            patch.Put(section);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(section);
        }

        // POST: odata/Sections
        public IHttpActionResult Post(Section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sections.Add(section);
            db.SaveChanges();

            return Created(section);
        }

        // PATCH: odata/Sections(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Section> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Section section = db.Sections.Find(key);
            if (section == null)
            {
                return NotFound();
            }

            patch.Patch(section);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(section);
        }

        // DELETE: odata/Sections(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Section section = db.Sections.Find(key);
            if (section == null)
            {
                return NotFound();
            }

            db.Sections.Remove(section);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Sections(5)/Property
        [EnableQuery]
        public SingleResult<Property> GetProperty([FromODataUri] int key)
        {
            return SingleResult.Create(db.Sections.Where(m => m.Id == key).Select(m => m.Property));
        }

        // GET: odata/Sections(5)/WorkItemList
        [EnableQuery]
        public IQueryable<WorkItem> GetWorkItemList([FromODataUri] int key)
        {
            return db.Sections.Where(m => m.Id == key).SelectMany(m => m.WorkItemList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SectionExists(int key)
        {
            return db.Sections.Count(e => e.Id == key) > 0;
        }
    }
}

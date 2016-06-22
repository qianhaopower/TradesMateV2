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
    builder.EntitySet<WorkItemTemplate>("WorkItemTemplates");
    builder.EntitySet<Company>("Companies"); 
    builder.EntitySet<WorkItem>("WorkItems"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class WorkItemTemplatesController : ODataController
    {
        private EFDbContext db = new EFDbContext();

        // GET: odata/WorkItemTemplates
        [EnableQuery]
        public IQueryable<WorkItemTemplate> GetWorkItemTemplates()
        {
            return db.WorkItemTemplates;
        }

        // GET: odata/WorkItemTemplates(5)
        [EnableQuery]
        public SingleResult<WorkItemTemplate> GetWorkItemTemplate([FromODataUri] int key)
        {
            return SingleResult.Create(db.WorkItemTemplates.Where(workItemTemplate => workItemTemplate.Id == key));
        }

        // PUT: odata/WorkItemTemplates(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<WorkItemTemplate> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkItemTemplate workItemTemplate = db.WorkItemTemplates.Find(key);
            if (workItemTemplate == null)
            {
                return NotFound();
            }

            patch.Put(workItemTemplate);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkItemTemplateExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(workItemTemplate);
        }

        // POST: odata/WorkItemTemplates
        public IHttpActionResult Post(WorkItemTemplate workItemTemplate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WorkItemTemplates.Add(workItemTemplate);
            db.SaveChanges();

            return Created(workItemTemplate);
        }

        // PATCH: odata/WorkItemTemplates(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<WorkItemTemplate> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkItemTemplate workItemTemplate = db.WorkItemTemplates.Find(key);
            if (workItemTemplate == null)
            {
                return NotFound();
            }

            patch.Patch(workItemTemplate);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkItemTemplateExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(workItemTemplate);
        }

        // DELETE: odata/WorkItemTemplates(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            WorkItemTemplate workItemTemplate = db.WorkItemTemplates.Find(key);
            if (workItemTemplate == null)
            {
                return NotFound();
            }

            db.WorkItemTemplates.Remove(workItemTemplate);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/WorkItemTemplates(5)/Company
        [EnableQuery]
        public SingleResult<Company> GetCompany([FromODataUri] int key)
        {
            return SingleResult.Create(db.WorkItemTemplates.Where(m => m.Id == key).Select(m => m.Company));
        }

        // GET: odata/WorkItemTemplates(5)/WorkItemList
        [EnableQuery]
        public IQueryable<WorkItem> GetWorkItemList([FromODataUri] int key)
        {
            return db.WorkItemTemplates.Where(m => m.Id == key).SelectMany(m => m.WorkItemList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkItemTemplateExists(int key)
        {
            return db.WorkItemTemplates.Count(e => e.Id == key) > 0;
        }
    }
}

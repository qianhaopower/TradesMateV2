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
    builder.EntitySet<WorkItem>("WorkItems");
    builder.EntitySet<Section>("Sections"); 
    builder.EntitySet<WorkItemTemplate>("WorkItemTemplates"); 
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class WorkItemsController : ODataController
    {
        private EFDbContext db = new EFDbContext();

        // GET: odata/WorkItems
        [EnableQuery]
        public IQueryable<WorkItem> GetWorkItems()
        {
            return db.WorkItems;
        }

        // GET: odata/WorkItems(5)
        [EnableQuery]
        public SingleResult<WorkItem> GetWorkItem([FromODataUri] int key)
        {
            return SingleResult.Create(db.WorkItems.Where(workItem => workItem.Id == key));
        }

        // PUT: odata/WorkItems(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<WorkItem> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkItem workItem = db.WorkItems.Find(key);
            if (workItem == null)
            {
                return NotFound();
            }

            patch.Put(workItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(workItem);
        }

        // POST: odata/WorkItems
        public IHttpActionResult Post(WorkItem workItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            workItem.AddedDate = DateTime.Now;
            workItem.ModifiedDate = DateTime.Now;

            db.WorkItems.Add(workItem);
            db.SaveChanges();

            return Created(workItem);
        }

        // PATCH: odata/WorkItems(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<WorkItem> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WorkItem workItem = db.WorkItems.Find(key);
            if (workItem == null)
            {
                return NotFound();
            }

            patch.Patch(workItem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(workItem);
        }

        // DELETE: odata/WorkItems(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            WorkItem workItem = db.WorkItems.Find(key);
            if (workItem == null)
            {
                return NotFound();
            }

            db.WorkItems.Remove(workItem);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/WorkItems(5)/Section
        [EnableQuery]
        public SingleResult<Section> GetSection([FromODataUri] int key)
        {
            return SingleResult.Create(db.WorkItems.Where(m => m.Id == key).Select(m => m.Section));
        }

        // GET: odata/WorkItems(5)/TemplateRecord
        [EnableQuery]
        public SingleResult<WorkItemTemplate> GetTemplateRecord([FromODataUri] int key)
        {
            return SingleResult.Create(db.WorkItems.Where(m => m.Id == key).Select(m => m.TemplateRecord));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WorkItemExists(int key)
        {
            return db.WorkItems.Count(e => e.Id == key) > 0;
        }
    }
}

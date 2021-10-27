using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using MVCWeb_WebAPI.Data;
using MVCWeb_WebAPI.Models;

namespace MVCWeb_WebAPI.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using MVCWeb_WebAPI.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<employee>("employees");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class employeesController : ODataController
    {
        private MVCWeb_WebAPIDbContext db = new MVCWeb_WebAPIDbContext();

        // GET: odata/employees
        [EnableQuery]
        public IQueryable<employee> Getemployees()
        {
            return db.employees;
        }

        // GET: odata/employees(5)
        [EnableQuery]
        public SingleResult<employee> Getemployee([FromODataUri] int key)
        {
            return SingleResult.Create(db.employees.Where(employee => employee.EId == key));
        }

        // PUT: odata/employees(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<employee> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employee employee = db.employees.Find(key);
            if (employee == null)
            {
                return NotFound();
            }

            patch.Put(employee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employeeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(employee);
        }

        // POST: odata/employees
        public IHttpActionResult Post(employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.employees.Add(employee);
            db.SaveChanges();

            return Created(employee);
        }

        // PATCH: odata/employees(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<employee> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employee employee = db.employees.Find(key);
            if (employee == null)
            {
                return NotFound();
            }

            patch.Patch(employee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!employeeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(employee);
        }

        // DELETE: odata/employees(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            employee employee = db.employees.Find(key);
            if (employee == null)
            {
                return NotFound();
            }

            db.employees.Remove(employee);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool employeeExists(int key)
        {
            return db.employees.Count(e => e.EId == key) > 0;
        }
    }
}

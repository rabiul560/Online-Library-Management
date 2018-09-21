﻿using System;
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
using MyLibraryApi.Models;
using System.Web.Http.Cors;

namespace MyLibraryApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Library_Management_API.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Purchase>("Purchases");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
      [EnableCors("*", "*", "*", "*")]
      //[Authorize(Roles = "Admin")]
    public class PurchasesController : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/Purchases
        [EnableQuery]
        public IQueryable<Purchase> GetPurchases()
        {
            return db.Purchase;
        }

        // GET: odata/Purchases(5)
        [EnableQuery]
        public SingleResult<Purchase> GetPurchase([FromODataUri] int key)
        {
            return SingleResult.Create(db.Purchase.Where(purchase => purchase.PurId == key));
        }

        // PUT: odata/Purchases(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Purchase> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Purchase purchase = db.Purchase.Find(key);
            if (purchase == null)
            {
                return NotFound();
            }

            patch.Put(purchase);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(purchase);
        }

        // POST: odata/Purchases
        public IHttpActionResult Post(Purchase purchase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Purchase.Add(purchase);
            db.SaveChanges();

            return Created(purchase);
        }

        // PATCH: odata/Purchases(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Purchase> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Purchase purchase = db.Purchase.Find(key);
            if (purchase == null)
            {
                return NotFound();
            }

            patch.Patch(purchase);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(purchase);
        }

        // DELETE: odata/Purchases(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Purchase purchase = db.Purchase.Find(key);
            if (purchase == null)
            {
                return NotFound();
            }

            db.Purchase.Remove(purchase);
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

        private bool PurchaseExists(int key)
        {
            return db.Purchase.Count(e => e.PurId == key) > 0;
        }
    }
}

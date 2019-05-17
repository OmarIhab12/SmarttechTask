using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using SmarttechTask.Models;
using Syncfusion.XlsIO;

namespace SmarttechTask.Controllers
{
    public class ProductsController : Controller
    {
        private Entities db = new Entities();

        // GET: Products
        public ActionResult Index(string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                return View(db.Products.Where(p => p.Name.Contains(searchString)).ToList());
            }
            return View(db.Products.ToList());
        }
        

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string productString, HttpPostedFileBase photo)
        {
            Product product = JsonConvert.DeserializeObject<Product>(productString);

            if(photo != null)
            {
                product.Photo = new byte[photo.ContentLength];
                photo.InputStream.Read(product.Photo, 0, photo.ContentLength);
            }
            product.Id = (db.Products.Count() > 0 ? db.Products.Max(p => p.Id) : 0) + 1;

            product.LastUpdate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string productString, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                Product editedProduct = JsonConvert.DeserializeObject<Product>(productString);
                Product product = db.Products.SingleOrDefault(p => p.Id == editedProduct.Id);
                product.Name = editedProduct.Name;
                product.Price = editedProduct.Price;
                if (photo != null)
                {
                    product.Photo = new byte[photo.ContentLength];
                    photo.InputStream.Read(product.Photo, 0, photo.ContentLength);
                }
                product.LastUpdate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(/*product*/);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        

    }
}

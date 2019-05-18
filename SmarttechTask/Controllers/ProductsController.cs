using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
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
        
        public void ExportToExcel()
        {
            List<Product> products = db.Products.ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("catalog");

            ws.Cells["A1"].Value = "Name";
            ws.Cells["B1"].Value = "Photo";
            ws.Cells["C1"].Value = "Price";

            int rowStart = 3;
            foreach(var product in products)
            {
                //ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                MemoryStream ms = new MemoryStream(product.Photo);
                Image image = Image.FromStream(ms);

                ws.Row(rowStart).Height = 100;

                ExcelPicture pic = ws.Drawings.AddPicture(product.Id.ToString(), image);
                pic.SetPosition(rowStart-1,0, 1,0);
                pic.SetSize(100, 100);


                ws.Cells[string.Format("A{0}", rowStart)].Value = product.Name;
                //ws.Cells[string.Format("B{0}", rowStart)].Value = 
                ws.Cells[string.Format("C{0}", rowStart)].Value = product.Price;

                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            ws.Column(2).Width = 25;
            //Response.Clear();
            //Response.ContentType = "applecation/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("content-disposition", "attachement: filename=" + "ExcelCatalog.xlsx");
            //Response.BinaryWrite(pck.GetAsByteArray());
            //Response.End();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
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

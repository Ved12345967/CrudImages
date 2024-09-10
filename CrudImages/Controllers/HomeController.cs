using CrudImages.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;


namespace CrudImages.Controllers
{
    public class HomeController : Controller
    {
        EmployeeDBEntities db = new EmployeeDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            var data = db.employees.ToList();
            return View(data);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(employee e)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                string extension = Path.GetExtension(e.ImageFile.FileName);
                HttpPostedFileBase postedFile = e.ImageFile;
                int length = postedFile.ContentLength;
                fileName = fileName + extension;
                e.Image_Path = "~/images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                e.ImageFile.SaveAs(fileName);
                db.employees.Add(e);
                db.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        public ActionResult Edit(int id)
        {
            var EmployeeRow=db.employees.Where(model=>model.Id==id).FirstOrDefault();
            Session["Image"]= EmployeeRow.Image_Path;
            return View(EmployeeRow);
        }
        [HttpPost]
        public ActionResult Edit(employee e)
        {
            if (ModelState.IsValid == true)
            {
                if(e.ImageFile !=null)
                {

                    string fileName = Path.GetFileNameWithoutExtension(e.ImageFile.FileName);
                    string extension = Path.GetExtension(e.ImageFile.FileName);
                    HttpPostedFileBase postedFile = e.ImageFile;
                    int length = postedFile.ContentLength;
                    fileName = fileName + extension;
                    e.Image_Path = "~/images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                    e.ImageFile.SaveAs(fileName);
                    db.Entry(e).State=EntityState .Modified;
                    db.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    e.Image_Path = Session["image"].ToString();
                    db.Entry(e).State = EntityState.Modified;
                    db.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Index", "Home");
                }
            }
           
            return View();
        }
        public ActionResult Delete(int id)
        {
            if(id > 0)
            {
                var EmployeeRow = db.employees.Where(model => model.Id == id).FirstOrDefault();
                if(EmployeeRow !=null)
                {
                    db.Entry(EmployeeRow).State = EntityState.Deleted; 
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

    }
}
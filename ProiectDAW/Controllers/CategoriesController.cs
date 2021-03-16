using ProiectDAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Category
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var categorii = from categorie in db.Categories
                            orderby categorie.CategorieNume
                            select categorie;
            ViewBag.Categorii = categorii;
            return View();
        }

        public ActionResult Show(int id)
        {
            Category categorie = db.Categories.Find(id);
            return View(categorie);
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Category cat)
        {
            try
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View(cat);
            }
        }

        public ActionResult Edit(int id)
        {
            Category categorie = db.Categories.Find(id);
            return View(categorie);
        }

        [HttpPut]
        public ActionResult Edit(int id, Category requestCategorie)
        {
            try
            {
                Category categorie = db.Categories.Find(id);
                if (TryUpdateModel(categorie))
                {
                    categorie.CategorieNume = requestCategorie.CategorieNume;
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost modificata!";
                    return RedirectToAction("Index");
                }
                return View(requestCategorie);

            }
            catch (Exception e)
            {
                return View(requestCategorie);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category categorie = db.Categories.Find(id);
            db.Categories.Remove(categorie);
            TempData["message"] = "Categoria a fost stearsa!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

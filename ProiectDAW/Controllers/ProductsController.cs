using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;



namespace ProiectDAW.Controllers
{
    //[Authorize]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Article
        //[Authorize(Roles = "User,Colab,Admin")]
        //public ActionResult Index()
        //{
          //  var produse = db.Products.Include("Category").Include("User");
            //ViewBag.Produse = produse;

            //if (TempData.ContainsKey("message"))
           // {
             //   ViewBag.Message = TempData["message"];
           // }

         //   return View();
       // }

        // GET: Article
        //[Authorize(Roles = "User,Colab,Admin")]
        public ActionResult Index(string searchString)
        {
            var produse = from p in db.Products.Include("Category").Include("User").Where(x => x.needsApproval == false)
                        select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                produse = produse.Where(p => p.Titlu.Contains(searchString)).OrderBy(p => p.Titlu);
                if (produse.Count() == 0)
                    ViewBag.Message = "Nu a fost gasit niciun produs";
            }

            
            ViewBag.Produse = produse;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(produse);
        }

        // GET: Article
        //[Authorize(Roles = "User,Colab,Admin")]
        /*  public ActionResult Index2(string sortOrder)
          {
              ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "Nume_Desc" : "";


              var produse = from p in db.Products.Include("Category").Include("User")
                            select p;

              switch(sortOrder)
              {
                  case "Nume_Desc":
                      produse = produse.OrderByDescending(p => p.Titlu);
                      break;
                  default:
                      produse = produse.OrderBy(p => p.Titlu);
                      break;

              }

              ViewBag.Produse = produse;
              if (TempData.ContainsKey("message"))
              {
                  ViewBag.Message = TempData["message"];
              }

              return View(produse);
          }
          */

        public ActionResult Index2()
        {
           var produse = from p in db.Products.Include("Category").Include("User")
                          select p;
            produse = produse.OrderBy(p => p.Titlu);
            ViewBag.Produse = produse;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(produse);
        }

        public ActionResult Index3()
        {
            var produse = from p in db.Products.Include("Category").Include("User")
                          select p;
            produse = produse.OrderByDescending(p => p.Titlu);
            ViewBag.Produse = produse;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(produse);
        }

        public ActionResult Index4()
        {
            var produse = from p in db.Products.Include("Category").Include("User")
                          select p;
            produse = produse.OrderBy(p => p.Rating);
            ViewBag.Produse = produse;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(produse);
        }

        public ActionResult Index5()
        {
            var produse = from p in db.Products.Include("Category").Include("User")
                          select p;
            produse = produse.OrderByDescending(p => p.Rating);
            ViewBag.Produse = produse;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(produse);
        }

        public ActionResult IndexRequest()
        {
            var produse = from p in db.Products.Include("Category").Include("User").Where(x => x.needsApproval == true)
                          select p;

            ViewBag.Produse = produse;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }

            return View(produse);
        }

        //[Authorize(Roles = "User,Colab,Admin")]

        public ActionResult Show(int id)
        {
            Product produs = db.Products.Find(id);
            decimal rating = 0;

            foreach (Review r in produs.Reviews)
            {
                rating = rating + r.Nota;
            }

            if (rating > 0)
            {
                rating = rating / produs.Reviews.Count;
                produs.Rating = Decimal.Round(rating, 2);
                db.SaveChanges();
            }

            SetAccessRights();
            return View(produs);

        }

        [HttpPost]
       // [Authorize(Roles = "User,Colab,Admin")]
        public ActionResult Show(Review rev)
        {
            rev.Date = DateTime.Now;
            rev.UserId = User.Identity.GetUserId();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Reviews.Add(rev);
                    db.SaveChanges();
                    return Redirect("/Products/Show/" + rev.ProdusID);
                }
                else
                {
                    Product prod = db.Products.Find(rev.ProdusID);
                    SetAccessRights();
                    return View(prod);
                }
            }

            catch (Exception e)
            {
                Product prod = db.Products.Find(rev.ProdusID);
                SetAccessRights();
                return View(prod);
            }

        }


        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            Product produs = new Product();
            produs.Categ = GetAllCategories();

            produs.UserId = User.Identity.GetUserId();
            produs.needsApproval = false;

            return View(produs);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult New(Product produs)
        {
            produs.UserId = User.Identity.GetUserId();
            produs.needsApproval = false;

            try
            {
                if (ModelState.IsValid)
                {
                    // Adaugare Imagine

                    string fileName = Path.GetFileNameWithoutExtension(produs.ImageFile.FileName);
                    string extension = Path.GetExtension(produs.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    produs.ImagePath = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);

                    produs.ImageFile.SaveAs(fileName);

                    db.Products.Add(produs);
                    db.SaveChanges();
                    TempData["message"] = "Produsul a fost adaugat cu succes";

                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    produs.Categ = GetAllCategories();
                    return View(produs);
                }
            }
            catch (Exception e)
            {
                produs.Categ = GetAllCategories();
                return View(produs);
            }
        }

        [Authorize(Roles = "Colab")]
        public ActionResult NewRequest()
        {
            Product produs = new Product();
            produs.Categ = GetAllCategories();

            produs.UserId = User.Identity.GetUserId();
            produs.needsApproval = true;

            return View(produs);
        }

        [HttpPost]
        [Authorize(Roles = "Colab")]
        public ActionResult NewRequest(Product produs)
        {
            produs.UserId = User.Identity.GetUserId();
            produs.needsApproval = true;

            try
            {
                if (ModelState.IsValid)
                {
                    // Adaugare Imagine

                    string fileName = Path.GetFileNameWithoutExtension(produs.ImageFile.FileName);
                    string extension = Path.GetExtension(produs.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    produs.ImagePath = "~/Image/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);

                    produs.ImageFile.SaveAs(fileName);

                    db.Products.Add(produs);
                    db.SaveChanges();
                    TempData["message"] = "Produsul a fost adaugat cu succes";

                    ModelState.Clear();
                    return RedirectToAction("Index");
                }
                else
                {
                    produs.Categ = GetAllCategories();
                    return View(produs);
                }
            }
            catch (Exception e)
            {
                produs.Categ = GetAllCategories();
                return View(produs);
            }
        }

        [Authorize(Roles = "Colab,Admin")]
        public ActionResult Edit(int id)
        {

            Product produs = db.Products.Find(id);
            produs.Categ = GetAllCategories();

            if (User.Identity.GetUserId() == produs.UserId || User.IsInRole("Admin"))
            {
                return View(produs);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa modificati produsul";
                return RedirectToAction("Index");
            }
        }


        [HttpPut]
        [Authorize(Roles = "Colab,Admin")]
        public ActionResult Edit(int id, Product requestProdus)
        {
            

            try
            {
                if (ModelState.IsValid)
                {
                    Product produs = db.Products.Find(id);
                    if (User.Identity.GetUserId() == produs.UserId || User.IsInRole("Admin"))
                    {

                        if (TryUpdateModel(produs))
                        {
                            produs.ImageFile = requestProdus.ImageFile;
                            produs.ImagePath = requestProdus.ImagePath;
                            produs.Titlu = requestProdus.Titlu;
                            produs.Descriere = requestProdus.Descriere;
                            produs.Pret = requestProdus.Pret;
                            produs.Rating = requestProdus.Rating;
                            produs.CategorieID = requestProdus.CategorieID;
                            db.SaveChanges();
                            TempData["message"] = "Produsul a fost modificat";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa modificati produsul";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    requestProdus.Categ = GetAllCategories();
                    return View(requestProdus);
                }

            }
            catch (Exception e)
            {
                requestProdus.Categ = GetAllCategories();
                return View(requestProdus);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Colab,Admin")]
        public ActionResult Delete(int id)
        {
            Product produs = db.Products.Find(id);
            if (User.Identity.GetUserId() == produs.UserId || User.IsInRole("Admin"))
            {
                db.Products.Remove(produs);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost sters";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti produsul";
                return RedirectToAction("Index");
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();

            var categoriile = from cat in db.Categories
                              select cat;

            foreach (var categorie in categoriile)
            {
                selectList.Add(new SelectListItem
                {
                    Value = categorie.CategorieID.ToString(),
                    Text = categorie.CategorieNume.ToString()
                });
            }

            return selectList;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult AcceptRequest(int id)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Product req = db.Products.Find(id);

                    if (TryUpdateModel(req))
                    {
                        req.needsApproval = false;
                        db.SaveChanges();
                    }
                } 
            } catch (Exception e) { }

            return RedirectToAction("Index");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRequest(int id)
        {
            Product produs = db.Products.Find(id);
            db.Products.Remove(produs);
            db.SaveChanges();
            TempData["message"] = "Produsul a fost sters";
            return RedirectToAction("IndexRequest");
        }

        private void SetAccessRights()
        {
            ViewBag.afisareButoane = false;
            if (User.IsInRole("Colab") || User.IsInRole("Admin"))
            {
                ViewBag.afisareButoane = true;
            }
            if (User.IsInRole("Colab") || User.IsInRole("Admin") || User.IsInRole("User"))
            {
                ViewBag.areCont = true;
            }

            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();
        }



    }
}
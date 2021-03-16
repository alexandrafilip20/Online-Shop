using Microsoft.AspNet.Identity;
using ProiectDAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProiectDAW.Controllers
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "User,Colab,Admin")]
        public ActionResult Delete(int id)
        {
            Review rev = db.Reviews.Find(id);

            if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Reviews.Remove(rev);
                db.SaveChanges();
                return Redirect("/Products/Show/" + rev.ProdusID);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti comentariul";
                return RedirectToAction("Index", "Products");
            }
        }

        [Authorize(Roles = "User,Colab,Admin")]
        public ActionResult Edit(int id)
        {
            Review rev = db.Reviews.Find(id);
            if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(rev);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Products");
            }
            
        }


        [HttpPut]
        [Authorize(Roles = "User,Colab,Admin")]
        public ActionResult Edit(int id, Review requestReview)
        {
            try
            {
                Review rev = db.Reviews.Find(id);
                if (rev.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                {
                    if (TryUpdateModel(rev))
                    {
                        rev.Content = requestReview.Content;
                        rev.Nota = requestReview.Nota;
                        db.SaveChanges();
                    }
                    return Redirect("/Products/Show/" + rev.ProdusID);
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                    return RedirectToAction("Index", "Products");
                }
               
            }
            catch (Exception e)
            {
                return View(requestReview);
            }

        }
    }
}
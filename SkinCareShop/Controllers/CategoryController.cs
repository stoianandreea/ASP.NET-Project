using SkinCareShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinCareShop.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //////////////////////////////////////////////// READ /////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult Index() {
            List<Category> categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        //////////////////////////////////////////////// CREATE /////////////////////////////////////////////////////////////////////
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New() {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public ActionResult New(Category categoryRequest) {
            try {
                if (ModelState.IsValid) {
                    db.Categories.Add(categoryRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(categoryRequest);
            }
            catch {
                return View(categoryRequest);
            }
        }

        //////////////////////////////////////////////// UPDATE /////////////////////////////////////////////////////////////////////
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id.HasValue) {
                Category category = db.Categories.Find(id);
                if (category == null) {
                    return HttpNotFound("Couldn't find the category with id " + id.ToString());
                }
                return View(category);
            }
            return HttpNotFound("Missing category id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Category categoryRequest) {
            try {
                if (ModelState.IsValid) {
                    Category category = db.Categories.SingleOrDefault(b => b.CategoryId.Equals(id));
                    if (TryUpdateModel(category)) {
                        category.Name = categoryRequest.Name;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(categoryRequest);
            }
            catch (Exception) {
                return View(categoryRequest);
            }
        }

        //////////////////////////////////////////////// DELETE /////////////////////////////////////////////////////////////////////
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id) {
            Category category = db.Categories.Find(id);
            if (category != null) {
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the category with id " + id.ToString());
        }
    }
}
using Microsoft.AspNet.Identity;
using SkinCareShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinCareShop.Controllers {
    public class ProductController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();

        //////////////////////////////////////////////// READ /////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult Index() {
            List<Product> products = db.Products.Include("Manufacturer").Include("Category").ToList();
            ViewBag.Products = products;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id) {
            if (id.HasValue) {
                Product product = db.Products.Find(id);
                if (product != null) {
                    return View(product);
                }
                return HttpNotFound("Couldn't find the product with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing product id parameter!");
        }

        //////////////////////////////////////////////// CREATE /////////////////////////////////////////////////////////////////////
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult New() {
            Product product = new Product();
            product.ManufacturersList = GetAllManufacturers();
            product.CategoriesList = GetAllCategories();
            return View(product);
        }

        [HttpPost]
        public ActionResult New(Product productRequest) {
            try {
                productRequest.ManufacturersList = GetAllManufacturers();
                productRequest.CategoriesList = GetAllCategories();
                if (ModelState.IsValid) {
                    db.Products.Add(productRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(productRequest);
            }
            catch {
                return View(productRequest);
            }
        }

        //////////////////////////////////////////////// UPDATE /////////////////////////////////////////////////////////////////////
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id.HasValue) {
                Product product = db.Products.Find(id);
                if (product == null) {
                    return HttpNotFound("Couldn't find the product with id " + id.ToString());
                }
                product.ManufacturersList = GetAllManufacturers();
                product.CategoriesList = GetAllCategories();
                return View(product);
            }
            return HttpNotFound("Missing product id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Product productRequest) {
            try {
                productRequest.ManufacturersList = GetAllManufacturers();
                productRequest.CategoriesList = GetAllCategories();
                if (ModelState.IsValid) {
                    Product product = db.Products.Include("Manufacturer").SingleOrDefault(b => b.ProductId.Equals(id));
                    if (TryUpdateModel(product)) {
                        product.Name = productRequest.Name;
                        product.Upc = productRequest.Upc;
                        product.Quantity = productRequest.Quantity;
                        product.Description = productRequest.Description;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(productRequest);
            }
            catch (Exception) {
                return View(productRequest);
            }
        }

        //////////////////////////////////////////////// DELETE /////////////////////////////////////////////////////////////////////
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id) {
            Product product = db.Products.Find(id);
            if (product != null) {
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the product with id " + id.ToString());
        }

        //////////////////////////////////////////////// ORDER /////////////////////////////////////////////////////////////////////

        [HttpGet]
        public ActionResult Order(int? id) {
            if (id.HasValue) {
                Product product = db.Products.Find(id);
                if (product == null) {
                    return HttpNotFound("Couldn't find the product with id " + id.ToString());
                }
                return View(product);
            }
            return HttpNotFound("Missing product id parameter!");
        }

        [HttpPut]
        public ActionResult Order(int id) {
            string userId = User.Identity.GetUserId();
            Product product = db.Products.Find(id);

            if (TryUpdateModel(product)) {
                if (product.Quantity >= 1) {
                    product.Users.Add(db.Users.Find(userId));
                    product.Quantity -= 1;
                    db.SaveChanges();
                }
                else {
                    return HttpNotFound("There are not enough products");
                }
            }
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllManufacturers() {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            foreach (var manuf in db.Manufacturers.ToList()) {
                // adaugam in lista elementele necesare pt dropdown
                selectList.Add(new SelectListItem {
                    Value = manuf.ManufacturerId.ToString(),
                    Text = manuf.Name
                });
            }
            // returnam lista pentru dropdown
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories() {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            foreach (var category in db.Categories.ToList()) {
                // adaugam in lista elementele necesare pt dropdown
                selectList.Add(new SelectListItem {
                    Value = category.CategoryId.ToString(),
                    Text = category.Name
                });
            }
            // returnam lista pentru dropdown
            return selectList;
        }
    }
}
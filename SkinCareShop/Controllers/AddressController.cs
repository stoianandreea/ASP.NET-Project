using SkinCareShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinCareShop.Controllers
{
    public class AddressController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        public ActionResult Index()
        {
            List<Address> addresses = db.Addresses.ToList();
            ViewBag.Addresses = addresses;
            return View();
        }

        /////////////////////////////////////////////////// CREATE //////////////////////////////////////////////////
        [HttpGet]
        public ActionResult New() {
            Address address = new Address();
            return View(address);
        }

        [HttpPost]
        public ActionResult New(Address addressRequest) {
            try {
                if (ModelState.IsValid) {
                    db.Addresses.Add(addressRequest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(addressRequest);
            }
            catch (Exception) {
                return View(addressRequest);
            }
        }

        //////////////////////////////////////////////// UPDATE /////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id.HasValue) {
                Address address = db.Addresses.Find(id);
                if (address == null) {
                    return HttpNotFound("Couldn't find the address with id " + id.ToString());
                }
                return View(address);
            }
            return HttpNotFound("Missing address id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, Address addressRequest) {
            try {
                if (ModelState.IsValid) {
                    Address address = db.Addresses.SingleOrDefault(b => b.AdressId.Equals(id));
                    if (TryUpdateModel(address)) {
                        address.Country = addressRequest.Country;
                        address.City = addressRequest.City;
                        address.StreetNumber = addressRequest.StreetNumber;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(addressRequest);
            }
            catch (Exception) {
                return View(addressRequest);
            }
        }

        //////////////////////////////////////////////// DELETE /////////////////////////////////////////////////////////////////////
        [HttpDelete]
        public ActionResult Delete(int id) {
            Address address = db.Addresses.Find(id);
            if (address != null) {
                db.Addresses.Remove(address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the address with id " + id.ToString());
        }

    }
}
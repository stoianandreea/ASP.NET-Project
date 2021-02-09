using SkinCareShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinCareShop.Controllers
{
    public class ManufacturerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //////////////////////////////////////////////// READ /////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult Index() {
            List<Manufacturer> manufacturers = db.Manufacturers.ToList();
            ViewBag.Manufacturers = manufacturers;
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id) {
            if (id.HasValue) {
                Manufacturer manufacturer = db.Manufacturers.Find(id);
                if (manufacturer != null) {
                    return View(manufacturer);
                }
                return HttpNotFound("Couldn't find the manufacturer with id " + id.ToString() + "!");
            }
            return HttpNotFound("Missing manufacturer id parameter!");
        }

        //////////////////////////////////////////////// CREATE /////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult New() {
            ManufacturerAddressViewModel manadr = new ManufacturerAddressViewModel();
            return View(manadr);
        }

        [HttpPost]
        public ActionResult New(ManufacturerAddressViewModel manadrViewModel) {
            try {
                if (ModelState.IsValid) {
                    Address address = new Address {
                        Country = manadrViewModel.Country,
                        City = manadrViewModel.City,
                        StreetNumber = manadrViewModel.StreetNumber
                    };
                    db.Addresses.Add(address);
                    Manufacturer manufacturer = new Manufacturer {
                        Name = manadrViewModel.Name,
                        PhoneNumber = manadrViewModel.PhoneNumber,
                        Email = manadrViewModel.Email,
                        Address = address
                    };
                    db.Manufacturers.Add(manufacturer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(manadrViewModel);
            }
            catch {
                return View(manadrViewModel);
            }
        }

        //////////////////////////////////////////////// UPDATE /////////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult Edit(int? id) {
            if (id.HasValue) {
                Manufacturer manufacturer = db.Manufacturers.Find(id);
                
                if (manufacturer == null) {
                    return HttpNotFound("Couldn't find the manufacturer with id " + id.ToString());
                }

                Address address = db.Addresses.Find(manufacturer.Address.AdressId);

                ManufacturerAddressViewModel manadrViewModel = new ManufacturerAddressViewModel {
                    Name = manufacturer.Name,
                    PhoneNumber = manufacturer.PhoneNumber,
                    Email = manufacturer.Email,
                    Country = address.Country,
                    City = address.City,
                    StreetNumber = address.StreetNumber
                };
                
                return View(manadrViewModel);
            }
            return HttpNotFound("Missing manufacturer id parameter!");
        }

        [HttpPut]
        public ActionResult Edit(int id, ManufacturerAddressViewModel manadrViewModel) {
            try {
                if (ModelState.IsValid) {
                    Manufacturer manufacturer = db.Manufacturers.SingleOrDefault(b => b.ManufacturerId.Equals(id));
                    Address address = db.Addresses.Find(manufacturer.Address.AdressId);
                    if (TryUpdateModel(manufacturer) && TryUpdateModel(address)) {
                        manufacturer.Name = manadrViewModel.Name;
                        manufacturer.PhoneNumber = manadrViewModel.PhoneNumber;
                        manufacturer.Email = manadrViewModel.Email;
                        address.Country = manadrViewModel.Country;
                        address.City = manadrViewModel.City;
                        address.StreetNumber = manadrViewModel.StreetNumber;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
                return View(manadrViewModel);
            }
            catch (Exception) {
                return View(manadrViewModel);
            }
        }

        //////////////////////////////////////////////// DELETE /////////////////////////////////////////////////////////////////////
        [HttpDelete]
        public ActionResult Delete(int id) {
            Manufacturer manufacturer = db.Manufacturers.Find(id);
            Address address = db.Addresses.Find(manufacturer.Address.AdressId);
            if (manufacturer != null) {
                db.Manufacturers.Remove(manufacturer);
                db.Addresses.Remove(address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the manufacturer with id " + id.ToString());
        }
    }
}
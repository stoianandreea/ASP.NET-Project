﻿using SkinCareShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkinCareShop.Controllers {
    // poate fi accesat doar de catre Admin
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller {
        private ApplicationDbContext ctx = new ApplicationDbContext();
        public ActionResult Index() {
            ViewBag.UsersList = ctx.Users
            .OrderBy(u => u.UserName)
            .ToList();
            return View();
        }
        public ActionResult Details(string id) {
            if (String.IsNullOrEmpty(id)) {
                return HttpNotFound("Missing the id parameter!");
            }

            ApplicationUser user = ctx.Users
            .Include("Roles")
            .FirstOrDefault(u => u.Id.Equals(id));

            if (user != null) {
                ViewBag.UserRole = ctx.Roles
                .Find(user.Roles.First().RoleId).Name;
                return View(user);
            }
            return HttpNotFound("Cloudn't find the user with given id!");
        }

        /////////////////////////////////////////////////// CREATE ///////////////////////////////////////////////////

        [HttpGet]
        public ActionResult New() {
            ApplicationUser user = new ApplicationUser();
            return View(user);
        }

        [HttpPost]
        public ActionResult New(ApplicationUser userRequest) {
            try {
                if (ModelState.IsValid) {
                    ctx.Users.Add(userRequest);
                    ctx.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(userRequest);
            }
            catch {
                return View(userRequest);
            }
        }

        /////////////////////////////////////////////////// UPDATE ///////////////////////////////////////////////////
        public ActionResult Edit(string id) {
            if (String.IsNullOrEmpty(id)) {
                return HttpNotFound("Missing the id parameter!");
            }

            UserViewModel uvm = new UserViewModel();
            uvm.User = ctx.Users.Find(id);

            IdentityRole userRole = ctx.Roles.Find(uvm.User.Roles.First().RoleId);
            uvm.RoleName = userRole.Name;
            return View(uvm);
        }

        [HttpPut]
        public ActionResult Edit(string id, UserViewModel uvm) {
            ApplicationUser user = ctx.Users.Find(id);
            try {
                if (TryUpdateModel(user)) {
                    var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ctx));

                    foreach (var r in ctx.Roles.ToList()) {
                        um.RemoveFromRole(user.Id, r.Name);
                    }
                    um.AddToRole(user.Id, uvm.RoleName);

                    user.UserName = uvm.User.Email;
                    user.Email = uvm.User.Email;
                    ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception) {
                return View(uvm);
            }
        }

        /////////////////////////////////////////////////// DELETE ///////////////////////////////////////////////////
        [HttpDelete]
        public ActionResult Delete(string id) {
            ApplicationUser user = ctx.Users.Find(id);
            if (user != null) {
                ctx.Users.Remove(user);
                ctx.SaveChanges();
                return RedirectToAction("Index");
            }
            return HttpNotFound("Couldn't find the user with id " + id.ToString() + "!");
        }
    }
}
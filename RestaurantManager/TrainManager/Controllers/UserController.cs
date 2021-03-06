﻿using System;
using System.Linq;
using Data;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using ToDoManager.Models.Home;
using ToDoManager.Models.Users;
using ToDoManager.Utils;

namespace ToDoManager.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(IndexVM model)
        {
            if (HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser") == null)
                return RedirectToAction("Login", "Home");

            if (!HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser").IsAdmin)
                return RedirectToAction("Login", "Home");
        

            model.Page = model.Page <= 0
                ? 1
                : model.Page;

            model.ItemsPerPage = model.ItemsPerPage <= 0
                ? 10
                : model.ItemsPerPage;

            using RestaurantManagerContext context = new RestaurantManagerContext();
            model.Items = context.Users.OrderBy(i => i.Id)
                .Skip((model.Page - 1) * model.ItemsPerPage)
                .Take(model.ItemsPerPage)
                .ToList();

            model.PagesCount = (int)Math.Ceiling(
                context.Users.Count() / (double)model.ItemsPerPage
            );

            return View(model);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            //if (HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser") == null)
            //    return RedirectToAction("Login", "Home");

            //if (!HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser").IsAdmin)
            //    return RedirectToAction("Login", "Home");

            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateVM model)
        {
            //if (HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser") == null)
            //    return RedirectToAction("Login", "Home");

            //if (!HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser").IsAdmin)
            //    return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
                return View(model);

            if (model.Password == model.ConfirmPassword)
            {
                using RestaurantManagerContext context = new RestaurantManagerContext();
                if (context.Users.Count()==0)
                {
                    User item = new User
                    {
                        Username = model.Username,
                        Password = model.ConfirmPassword,
                        Email = model.Email,
                        IsAdmin = true
                    };

                    context.Users.Add(item);
                    context.SaveChanges();

                    
                }
                else
                {
                    User item = new User
                    {
                        Username = model.Username,
                        Password = model.ConfirmPassword,
                        Email = model.Email,
                        IsAdmin = false
                    };

                    context.Users.Add(item);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index", "User");
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser") == null)
                return RedirectToAction("Login", "Home");

            if (!HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser").IsAdmin)
                return RedirectToAction("Login", "Home");

            using RestaurantManagerContext context = new RestaurantManagerContext();
            User item = context.Users.Find(id);

            EditVM model = new EditVM
            {
                Id = item.Id,
                Username = item.Username,
                Email = item.Email,
                IsAdmin = item.IsAdmin
            };

            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditVM model)
        {
            if (HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser") == null)
                return RedirectToAction("Login", "Home");

            if (!HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser").IsAdmin)
                return RedirectToAction("Login", "Home");

            if (!ModelState.IsValid)
                return View(model);

            User item = new User
            {
                Id = model.Id,
                Username = model.Username,
                Email = model.Email,
                IsAdmin = model.IsAdmin
            };

            using RestaurantManagerContext context = new RestaurantManagerContext();
            context.Users.Update(item);
            context.SaveChanges();

            return RedirectToAction("Index", "User");
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser") == null)
                return RedirectToAction("Login", "Home");

            if (!HttpContext.Session.GetObjectFromJson<LoggedUser>("loggedUser").IsAdmin)
                return RedirectToAction("Login", "Home");

            using RestaurantManagerContext context = new RestaurantManagerContext();
            context.Users.Remove(context.Users.Find(id));
            context.SaveChanges();

            return RedirectToAction("Index", "User");
        }
    }
}
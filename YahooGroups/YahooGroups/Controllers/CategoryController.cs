﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YahooGroups.Models;

namespace YahooGroups.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var Categories = from category in db.Categories orderby category.Name select category;

            ViewBag.Categories = Categories;

            return View();
        }

        [HttpGet]
        public ActionResult Show(int categoryId)
        {
            var Category = db.Categories.Find(categoryId);

            if (Category == null)
            {
                TempData["message"] = "The requested category could not be found!";
                return View("ErrNoEnt");
            }

            return View(Category);
        }

        [HttpGet]
        public ActionResult New()
        {
            CategoryModel category = new CategoryModel();
            return View(category);
        }

        [HttpPost]
        public ActionResult New(CategoryModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception e)
            {
                TempData["message"] = "Can't create a new category right now! (" + e.Message + ")";
                return View(category);
            }
        }

        [HttpGet]
        public ActionResult Edit(int categoryId)
        {
            var Category = db.Categories.Find(categoryId);

            if (Category == null)
            {
                TempData["message"] = "Cannot edit categoy which does not exist!";

                return View("ErrNoEnt");
            }

            ViewBag.categoryId = categoryId;

            return View(Category);
        }

        [HttpPut]
        public ActionResult Edit(int categoryId, CategoryModel category)
        {
            var categoryToBeChanged = db.Categories.Find(categoryId);

            try
            {
                if (ModelState.IsValid)
                {
                    if (TryUpdateModel(categoryToBeChanged))
                    {
                        categoryToBeChanged.Name = category.Name;
                        db.SaveChanges();
                        TempData["message"] = "Category updated successfully!";
                    }
                    return RedirectToAction("Show", new { categoryId = categoryId });
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception e)
            {
                TempData["message"] = "Could not update the category! (" + e.Message + ")";
                return View(category);
            }
        }

        [HttpDelete]
        public ActionResult Delete (int categoryId)
        {
            var Category = db.Categories.Find(categoryId);

            if (Category == null)
            {
                TempData["message"] = "Can't delete category which does not exist!";
                return View();
            }

            try
            {
                db.Categories.Remove(Category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["message"] = "Could not delete the category! (" + e.Message + ")";
                return View();
            }
        }
    }
}
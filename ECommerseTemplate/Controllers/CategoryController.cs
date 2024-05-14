﻿using ECommerseTemplate.Data;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerseTemplate.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<CategoryModel> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
			return View();
		}

        [HttpPost]
		public IActionResult Create(CategoryModel obj)
		{
            if (ModelState.IsValid)
            {
				_db.Categories.Add(obj);
				_db.SaveChanges();
			    return RedirectToAction("Index");
			}

            return View();
           
		}
	}
}

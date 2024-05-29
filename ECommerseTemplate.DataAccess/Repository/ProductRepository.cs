﻿using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerseTemplate.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            // Manual update
            Product productFromDb = _db.Products.FirstOrDefault(s => s.Id == product.Id);
            if (productFromDb == null)
            {
                productFromDb.Title = product.Title;
                productFromDb.ISBN = product.ISBN;
                productFromDb.Price = product.Price;
                productFromDb.Price50 = product.Price50;
                productFromDb.Price100 = product.Price100;
                productFromDb.ListPrice = product.ListPrice;
                productFromDb.Author = product.Author;
                productFromDb.Description = product.Description;
                productFromDb.CategoryId = product.CategoryId;

                if (productFromDb.ImageUrl != null) 
                {
                    productFromDb.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}

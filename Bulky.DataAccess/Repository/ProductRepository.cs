using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            //_db.Products.Update(product);
            var productFromDB = _db.Products.FirstOrDefault(u => u.Id == product.Id);
            if(productFromDB != null)
            {
                productFromDB.Title = product.Title;
                productFromDB.ISBN = product.ISBN;
                productFromDB.Price = product.Price;
                productFromDB.Price50 = product.Price50;
                productFromDB.Price100 = product.Price100;
                productFromDB.Description = product.Description;
                productFromDB.CategoryId = product.CategoryId;
                productFromDB.Author = product.Author;
                productFromDB.CoverTypeId = product.CoverTypeId;
                if(product.ImageUrl != null)
                {
                    productFromDB.ImageUrl = product.ImageUrl;
                }
            }
        }
    }
}

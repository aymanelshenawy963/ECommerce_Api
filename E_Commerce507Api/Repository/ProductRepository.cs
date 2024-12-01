using E_Commerce507Api.Data;
using E_Commerce507Api.Models;
using E_Commerce507Api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce507Api.Repository
{
    public class ProductRepository : Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}

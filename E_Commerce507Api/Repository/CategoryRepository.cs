using E_Commerce507Api.Data;
using E_Commerce507Api.Models;
using E_Commerce507Api.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce507Api.Repository
{
    public class CategoryRepository: Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public  CategoryRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            this.dbContext=dbContext;
        }
        //any additional logic
    }
}

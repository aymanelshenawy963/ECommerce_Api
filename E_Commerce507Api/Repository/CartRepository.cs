using E_Commerce507Api.Data;
using E_Commerce507Api.Models;
using E_Commerce507Api.Repository.IRepository;

namespace E_Commerce507Api.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        //any additional logic
    }
}

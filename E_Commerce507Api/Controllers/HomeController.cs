using E_Commerce507Api.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce507Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public HomeController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var products = productRepository.GetAll();
            return Ok(products);
        }
        [HttpGet("Details")]
        public IActionResult Details(int productId)
        {
            var products = productRepository.GetOne(expression:e=>e.Id==productId);
            if (products != null)
            {
                return Ok(products);
            }
            return NotFound();  
        }
    }
}

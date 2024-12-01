using E_Commerce507Api.DTO;
using E_Commerce507Api.Models;
using E_Commerce507Api.Repository;
using E_Commerce507Api.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce507Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
        [HttpGet]
        public IActionResult Index(int Page = 1, string? search = null)
        {
            if (Page <= 0)
                Page = 1;

            var products = productRepository.GetAll();
            if (search != null)
            {
                search = search.Trim();

                products = products.Where(e => e.Name.Contains(search));
            }
            products = products.Skip((Page - 1) * 5).Take(5);
     
            if (products.Any())
            {
                return Ok(products);

            }
            return NoContent();
        }
        [HttpPost]
        public IActionResult Create(ProductImageDTO productImageDTO)
        {
            if (ModelState.IsValid)
            {
                if (productImageDTO.ImgUrl.Length > 0)//99656
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productImageDTO.ImgUrl.FileName);// 1.png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        productImageDTO.ImgUrl.CopyTo(stream);
                    }
                }
                var product = new Product()
                {
                    Name = productImageDTO.Name,
                    Price = productImageDTO.Price,
                    Quantity = productImageDTO.Quantity,
                    Rate = productImageDTO.Rate,
                    Description = productImageDTO.Description,
                    ImgUrl = productImageDTO.ImgUrl.FileName,
                    CategoryId = productImageDTO.CategoryId

                };
                productRepository.Add(product);
                productRepository.Commit();
                CookieOptions cookieOptions = new();
                cookieOptions.Expires = DateTime.Now.AddMinutes(1);
                Response.Cookies.Append("Success", "Add Product Successfully"/*, cookieOptions*/);
                //HttpContext.Session.SetString("Success", "Add Product Successfully");
                return Ok(product);
            }
            return BadRequest(productImageDTO);

        }

        [HttpGet("Edit ")]
        public IActionResult Edit(ProductImageDTO productImageDTO)
        {
            var oldProduct = productRepository.GetOne(expression:e => e.Id == productImageDTO.Id);

            if (ModelState.IsValid)
            {

                if (productImageDTO.ImgUrl != null && productImageDTO.ImgUrl.Length > 0)//99656
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productImageDTO.ImgUrl.FileName);// 1.png
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), " images", oldProduct.ImgUrl);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        productImageDTO.ImgUrl.CopyTo(stream);
                    }
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                    oldProduct.ImgUrl = fileName;
                }
                else
                {
                    oldProduct.ImgUrl = oldProduct.ImgUrl;
                }
                productRepository.Edit(oldProduct);
                productRepository.Commit();

                return Ok(oldProduct);
            }
            return BadRequest(productImageDTO);


        }


        [HttpDelete("Delete")]
        public IActionResult Delete(int productId)
        {
            var product = productRepository.GetOne(expression:e=>e.Id== productId);
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.ImgUrl);
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            productRepository.Delete(product);
            productRepository.Commit();
            return Ok((product));

        }
    }
}

using AutoMapper;
using E_Commerce507Api.DTO;
using E_Commerce507Api.Models;
using E_Commerce507Api.Repository;
using E_Commerce507Api.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Commerce507Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICartRepository cartRepository;
        private readonly IMapper mapper;

        public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository, IMapper mapper)
        {
            this.userManager = userManager;
            this.cartRepository = cartRepository;
            this.mapper = mapper;
        }
        [HttpPost("AddTOCart")]
        public IActionResult AddToCart(CartDTO cartDTO)
        {
            cartDTO.ApplicationUserId = userManager.GetUserId(User);

            if (cartDTO.ApplicationUserId != null)
            {
                var cart = mapper.Map<Cart> (cartDTO);
                var product = cartRepository.GetOne(expression: e => e.ApplicationUserId == cartDTO.ApplicationUserId && e.ProductId == cart.ProductId);

                if (product != null)
                {
                    product.count += cartDTO.Count;
                }
                else
                {
                    cartRepository.Add(cart);
                  
                }
                cartRepository.Commit();
                return Ok(cart);
            }
            return NotFound();  
        }
        [HttpGet]
        public IActionResult Index()
        {
            var user = userManager.GetUserId(User);
            if (user != null)
            {
                var carts = cartRepository.GetAll(expression:e => e.ApplicationUserId == user);
                if (carts != null)
                {
                    return Ok(carts);
                }
              return NotFound(carts);

            }
            return NotFound();


        }
        [HttpPut("Increment")]
        public IActionResult Increment(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);
            var product = cartRepository.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.count++;
                cartRepository.Commit();
                return Ok(product);
            }
            return NotFound();
        }
        [HttpPut("Decrement")]

        public IActionResult Decrement(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);
            var product = cartRepository.GetOne(expression: e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                product.count--;
                if (product.count > 0)
                    cartRepository.Commit();
                else
                    product.count = 1;

                return Ok(product);
            }
            return NotFound();

        }
        [HttpDelete("Delete")]
        public IActionResult Delete(int productId)
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var product = cartRepository.GetOne(null, e => e.ApplicationUserId == ApplicationUserId && e.ProductId == productId);

            if (product != null)
            {
                cartRepository.Delete(product);
                cartRepository.Commit();
                return Ok(product);
            }
            return NotFound();
        }

        [HttpPost("Pay")]
        public IActionResult Pay()
        {
            var ApplicationUserId = userManager.GetUserId(User);

            var cartProduct = cartRepository.GetAll([e=>e.Product], e => e.ApplicationUserId == ApplicationUserId).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in cartProduct)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                        UnitAmount = (long)item.Product.Price * 100,
                    },
                    Quantity = item.count,
                });
            }

            var service = new SessionService();
            var session = service.Create(options);
            return Ok(session.Url);
        }
    }
}


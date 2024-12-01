using E_Commerce507Api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce507Api.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Minimum length must be 3")]
        [MaxLength(50, ErrorMessage = "Minimum length must be 50")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, 1000000)]
        public decimal Price { get; set; }

        [ValidateNever]
        public string ImgUrl { get; set; }
        [Range(0, 5)]
        public double Rate { get; set; }
        [Range(1, 10000)]
        public double Quantity { get; set; }

        [Required]
        public int CategoryId { get; set; }
         

        [ValidateNever]
        public Category Category { get; set; }
    }
}

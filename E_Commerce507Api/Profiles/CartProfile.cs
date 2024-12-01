using AutoMapper;
using E_Commerce507Api.DTO;
using E_Commerce507Api.Models;

namespace E_Commerce507Api.Profiles
{
    public class CartProfile:Profile
    {
        public CartProfile()
        {
            CreateMap<CartDTO, Cart>();
        }
    }
}

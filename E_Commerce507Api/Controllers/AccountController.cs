using AutoMapper;
using E_Commerce507Api.DTO;
using E_Commerce507Api.Models;
using E_Commerce507Api.Utilty;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce507Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public AccountController(UserManager<ApplicationUser>userManager
            ,SignInManager<ApplicationUser>signInManager,RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
        [HttpPost("Regsister")]
        public async Task<IActionResult> Regsister(ApplicationUserDTO userDTO)
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(SD.AdminRole));
                await roleManager.CreateAsync(new(SD.CustomarRole));
            }
            var user = mapper.Map<ApplicationUser>(userDTO);
            //ApplicationUser user = new ApplicationUser()
            //{
            //   UserName=userDTO.Name,
            //   Email=userDTO.Email,
            //   Adderss=userDTO.Address
            //};
            var result = await userManager.CreateAsync(user,userDTO.Password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                await userManager.AddToRoleAsync(user, SD.CustomarRole);
                return Ok(userDTO);
            }
            return BadRequest(result.Errors);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        { 
        var user = await userManager.FindByNameAsync(loginDTO.UserName);
            if (user != null)
            {
                var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
                if (result)
                {
                    await signInManager.SignInAsync(user, loginDTO.RemeberMe);
                    return Ok(user);

                }
                else
                {
                    ModelState.AddModelError("", "invalid password Or user");
                }

            }
        
            return NotFound();  

        }
        [HttpDelete("Logout")]

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok();
        }
      


        }

    }
    


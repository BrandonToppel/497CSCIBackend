using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WhereToWatchAPI.Models;

namespace WhereToWatchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager;
        List<ApplicationUsers> _list = new List<ApplicationUsers>();
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager, 
            IConfiguration configuration, ApplicationDbContext context)
        {
            Configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        //Creates users using identity core
        public async Task<IActionResult> Register(ApplicationUsers Input)
        {
            //Create the user using properties from Application Users, and the built in ones from Identity.
            var user = new ApplicationUsers
            {
                UserName = Input.Email,
                Email = Input.Email,
                firstName = Input.firstName,
                lastName = Input.lastName,
            };
            IdentityResult result = await _userManager.CreateAsync(user, Input.password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return StatusCode(StatusCodes.Status201Created);
            }
        }
        private string GenerateJWT(ApplicationUsers users)
        {
            var _jwtKey = Configuration["Jwt:Key"];
            var _jwtIssuer = Configuration["Jwt:Issuer"];
            var _jwtAudience = Configuration["Jwt:Audience"];
            var secuirtyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(secuirtyKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, users.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, users.UserName),
                new Claim(ClaimTypes.Name, users.UserName)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetJWTCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(5),
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Append("jwtCookie", token, cookieOptions);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(ApplicationUsers user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.password, user.rememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {

                var _user = await _userManager.FindByNameAsync(user.UserName);
                var accessToken = GenerateJWT(user);
                SetJWTCookie(accessToken);
                //  await CreateCart(user);
                return Ok(accessToken);
            }
        }
    }
}

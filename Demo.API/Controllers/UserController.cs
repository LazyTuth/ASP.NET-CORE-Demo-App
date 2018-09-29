using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Demo.API.Interfaces;
using Demo.API.ModelDtos;
using Demo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Demo.API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public UserController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userDto) 
        {
            userDto.Username = userDto.Username.ToLower();
            if(await _repo.UserExist(userDto.Username))
                return BadRequest("Username already existed !!!");
            
            var userToCreate = new User();
            userToCreate.UserName = userDto.Username;

            var createdUser = _repo.Register(userToCreate, userDto.Password);

            return StatusCode(201);//Will update later
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userDto)
        {
            var userFromRepo = await _repo.Login(userDto.Username.ToLower(), userDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
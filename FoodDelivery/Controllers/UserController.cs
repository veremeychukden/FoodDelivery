using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entities;
using DTO.Requests;
using DTO.Responses;
using FoodDelivery.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, IOptionsMonitor<JwtConfig> optionsMonitor, IMapper autoMapper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _mapper = autoMapper;
            _roleManager = roleManager;
        }

        /// <summary>
        /// User registartion
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Response</returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return BadRequest(new RegistrationResponseDTO()
                    {
                        Errors = new List<string>()
                        {
                            "Email already in use"
                        },
                        Success = false
                    });
                }

                var newUser = new User() {Email = user.Email, UserName = user.Username, Firstname = user.FirstName, Lastname = user.LastName};
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
                var roleResult = await _userManager.AddToRoleAsync(createdUser, "User");
                if (isCreated.Succeeded)
                {
                    var jwtToken = GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponseDTO()
                    {
                        Success = true,
                        Token = jwtToken
                    });
                }
                else
                {
                    return BadRequest(new RegistrationResponseDTO()
                    {
                        Errors = isCreated.Errors.Select(t=> t.Description).ToList(),
                        Success = false
                    });
                }
            }

            return BadRequest(new RegistrationResponseDTO()
            {
                Errors = new List<string>()
                {
                    "Invalid payload"
                },
                Success = false
            });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("role", userRoles[0])
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO user)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if(existingUser == null) {
                    return BadRequest(new RegistrationResponseDTO(){
                        Errors = new List<string>() {
                            "Invalid login request"
                        },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if(!isCorrect) {
                    return BadRequest(new RegistrationResponseDTO(){
                        Errors = new List<string>() {
                            "Invalid login request"
                        },
                        Success = false
                    });
                }

                var jwtToken  =GenerateJwtToken(existingUser);

                return Ok(new RegistrationResponseDTO() {
                    Success = true,
                    Token = jwtToken
                });
            }

            return BadRequest(new RegistrationResponseDTO(){
                Errors = new List<string>() {
                    "Invalid payload"
                },
                Success = false
            });
        }
        
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var result = await _userRepository.GetUsersAsync();
            return _mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(result);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserDTO> GetUserById([FromRoute] string id)
        {
            var result = await _userRepository.GetUserByIdAsync(id);
            return _mapper.Map<User, UserDTO>(result);
        }

        
        
    }
}
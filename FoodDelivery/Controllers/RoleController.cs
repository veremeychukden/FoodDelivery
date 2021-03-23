using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Entities;
using DTO.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Create role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var exists = await _roleManager.FindByNameAsync(name);
                if (exists == null)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                    {
                        return Ok(new RoleResponse()
                        {
                            Role = name,
                            Success = true
                        });
                    }
                    else
                    {
                        return BadRequest(new RoleResponse()
                        {
                            Success = false,
                            Errors = result.Errors.Select(x => x.Description).ToList()
                        });
                    }
                }
                else
                {
                    return BadRequest(new RoleResponse()
                    {
                        Success = false,
                        Errors = new List<string>() {"Role already exists"}
                    });
                }
            }

            return BadRequest(new RoleResponse()
            {
                Success = false,
                Errors = new List<string>() {"Invalid role name"}
            });
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Delete role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Ok(new RoleResponse()
                    {
                        Role = role.Name,
                        Success = true
                    });
                }
                else
                {
                    return BadRequest(new RoleResponse()
                    {
                        Success = false,
                        Errors = result.Errors.Select(x => x.Description).ToList()
                    });
                }
            }

            return BadRequest(new RoleResponse()
            {
                Success = false,
                Errors = new List<string>() {"Role is not existing"}
            });
        }
    }
}
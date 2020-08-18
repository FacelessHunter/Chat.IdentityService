using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Identity.Domain.DTOs;
using Identity.Domain.DTOs.User;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{

    [Route("api/users")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private UserManager<User> _userManager { get; set; }

        public UserController(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }


        /// <summary>
        /// Return user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /users/{id}
        ///     
        /// </remarks>
        /// <param name="id"> user's id</param>
        /// <returns>user</returns>
        /// <response code="200">If successful returned user</response>
        /// <response code="404">If not found user</response> 
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserByIdAsync([FromRoute]string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<UserDTO>(user);

            return Ok(mappedUser);
        }


        /// <summary>
        /// Return user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /users/{username}
        ///     
        /// </remarks>
        /// <param name="username"> Username</param>
        /// <returns>user</returns>
        /// <response code="200">If successful returned user</response>
        /// <response code="404">If not found user</response> 
        [Authorize]
        [HttpGet("{username}/username")]
        public async Task<ActionResult<UserDTO>> GetUserByUsernameAsync([FromRoute]string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var mappedUser = _mapper.Map<UserDTO>(user);

            return Ok(mappedUser);
        }

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /users
        ///     {
        ///         "UserName": "Vasy_Pupkin",
        ///         "Email": "Pupkin@Vasy.com",
        ///         "Password": "ffnfnhD43$",
        ///         "PasswordConfirm": "ffnfnhD43$"
        ///     }
        ///     
        /// </remarks>
        /// <param name="userDto">user's dto</param>
        /// <returns>Created user</returns>
        /// <response code="200">If successful created new user</response>
        /// <response code="400">If didn't creat new user</response> 
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUserAsync([FromBody]CreateUserDTO userDto)
        {
            var mappedUser = _mapper.Map<User>(userDto);

            var result = await _userManager.CreateAsync(mappedUser, userDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            var createdUser = await _userManager.FindByNameAsync(userDto.UserName);

            var createdMappedUser = _mapper.Map<UserDTO>(createdUser);

            return Ok(createdMappedUser);
        }

        /// <summary>
        /// Delete a specific user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE: /users
        ///     
        /// </remarks>
        /// <returns>Status code</returns>
        /// <response code="204">If successful delated user</response>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _userManager.DeleteAsync(user);

            return NoContent();
        }

        /// <summary>
        /// Change password current user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /users/changepassword
        ///     {
        ///         "OldPassword": "Pass123$",
        ///         "NewPassword": "NewPass123$",
        ///         "ConfirmNewPassword": "NewPass123$"
        ///     }
        ///     
        /// </remarks>
        /// <param name="changePasswordDto">ChangePasswordDto</param>
        /// <returns>Status code</returns>
        /// <response code="200">If successful changed user's password</response>
        /// <response code="400">If failed changed user's password</response>
        
        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePasswordUserAsync([FromBody]ChangePasswordDto changePasswordDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok();
        }

        [Authorize(Policy = "internal")]
        [HttpGet("{username}/exist")]
        public async Task<ActionResult<string>> IsUserExistAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound($"Not found {username}");

            return Ok($"User {username} exist");
        }

    }
}
        
    

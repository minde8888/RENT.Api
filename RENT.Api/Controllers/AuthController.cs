using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using WTP.Api.Configuration.Requests;
using WTP.Data.Interfaces;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Dtos.Responses;
using WTP.Domain.Entities.Auth;
using WTP.Services.Services;

namespace WTP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;

        public AuthController(UserManager<ApplicationUser> userManager,
            TokenValidationParameters tokenValidationsParams,
            IUserRepository userRepository,
            AuthService authService)

        {
            _userManager = userManager;
            _tokenValidationParams = tokenValidationsParams;
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if (ModelState.IsValid)
            {
                var exist = _userManager.Users.Any(u => 
                u.PhoneNumber == 
                user.PhoneNumber || 
                u.Email == 
                user.Email);

                if (exist)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>()
                        {
                            "Email or phone number is already in use !!!"
                        },
                        Success = false
                    });
                }

                var newUser = new ApplicationUser()
                {
                    Roles = user.Role,
                    Email = user.Email,
                    UserName = _authService.StringRandom(),
                    PhoneNumber = user.PhoneNumber
                };

                var isCreated = await _userManager.CreateAsync(newUser, user.Password);

                if (isCreated.Succeeded)
                {
                    try
                    {
                        await _userManager.AddToRoleAsync(newUser, user.Role.ToString());
                        user.UserId = newUser.Id;
                        await _userRepository.AddUserAsync(user);

                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new RegistrationResponse()
                        {
                            Errors = new List<string>() {
                                "Error to add user in the DB !!!  " + ex
                            },
                            Success = false
                        });
                    }
                }
                else
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                        Success = false
                    });
                }
            }
            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>()
                    {
                    "Invalig payloade"
                    },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "The email address is incorrect. Please retry..."
                            },
                        Success = false
                    });
                }
                if (existingUser.IsDeleted)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "User account was deleted "
                            },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "The password is incorrect. Please try again."
                            },
                        Success = false
                    });
                }
                try
                {
                    var token = await _authService.GenerateJwtTokenAsync(existingUser);
                    String ImageSrc = String.Format("{0}://{1}{2}", Request.Scheme, Request.Host, Request.PathBase);
                    var result = await _authService.GetUserInfo(existingUser, token, ImageSrc);

                    return Ok(result);
                }
                catch (Exception)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>() {
                                "Server Error. Please contact support."
                            },
                        Success = false
                    });
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                        "Invalid payload"
                    },
                Success = false
            });
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.email);
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);
                bool emailHelper = await _authService.SendEmailPasswordReset(model, Request.Headers["origin"], token);
                if (emailHelper)
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Can’t find that email !!!" });
            }
            return BadRequest(new { message = "Problems with ForgotPasswordToken !!!" });
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                bool result = await _authService.NewPassword(model);
                if (result)
                {
                    return Ok(new { message = "Password reset successful, you can now login" });
                }
            }
            catch (Exception)
            {
                return BadRequest(new { message = "The link you followed has expired !!!" });
            }
            return BadRequest(new { message = "The email you tried to reach does not exist !!!" });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequests tokenRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    JwtSecurityTokenHandler jwtTokenHandler = new();

                    _tokenValidationParams.ValidateLifetime = false;
                    var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
                    _tokenValidationParams.ValidateLifetime = true;
                    var res = await _authService.VerifyToken(tokenRequest, principal, validatedToken);
                    if (res == null)
                    {
                        return BadRequest(new RegistrationResponse()
                        {
                            Errors = new List<string>() {
                            "Invalid tokens"
                            },
                            Success = false
                        });
                    }
                    return Ok(res);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }

            return BadRequest(new RegistrationResponse()
            {
                Errors = new List<string>() {
                "Invalid payload"
            },
                Success = false
            });
        }
    }
}
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Api.Configuration.Requests;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Dtos;
using RENT.Domain.Entities.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;

namespace RENT.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IUserService _userServices;
        private readonly ITokenService _tokenService;
        private readonly IResetPasswordService _resetPasswordService;

        public AuthController(TokenValidationParameters tokenValidationParams,
            IUserService userServices,
            ITokenService tokenService,
            IResetPasswordService resetPasswordService)
        {
            _tokenValidationParams = tokenValidationParams ?? throw new ArgumentNullException(nameof(tokenValidationParams));
            _userServices = userServices ?? throw new ArgumentNullException(nameof(userServices));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _resetPasswordService = resetPasswordService ?? throw new ArgumentNullException(nameof(resetPasswordService));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Signup")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            try
            {
                var result = await _userServices.CreateNewUserAsync(user);
                return Ok(result.Success);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() {
                                "Error to add user to the DB !!!  " + ex
                            },
                    Success = false
                });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<List<UserInformationDto>>> Login([FromBody] UserLoginRequest user)
        {
            try
            {
                var imageSrc = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var result = await _userServices.UserInfo(user, imageSrc);
                return Ok(result.UserInformationDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>() {
                                "Server Error. Please contact support." + ex
                            },
                    Success = false
                });
            }
        }

        [AllowAnonymous]
        [HttpPost("Forgot-Password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            try
            {
                var emailHelper = await _resetPasswordService.SendEmailPasswordReset(model, Request.Headers["origin"]);
                return Ok(emailHelper);
            }
            catch (SmtpFailedRecipientException sx)
            {
                return BadRequest(new { message = "Email was not send " + sx });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Something is wrong " + ex });
            }
        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            try
            {
                var result = await _resetPasswordService.NewPassword(model);
                if (result)
                    return Ok(new { message = "Password reset successful, you can now login" });

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest(new { message = "The link you followed has expired !!!" });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequests tokenRequest)
        {
            try
            {
                JwtSecurityTokenHandler jwtTokenHandler = new();

                _tokenValidationParams.ValidateLifetime = false;
                var principal = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);
                _tokenValidationParams.ValidateLifetime = true;
                var res = await _tokenService.VerifyToken(tokenRequest, principal, validatedToken);
                if (res == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                            "Invalid tokens"
                            },
                        Success = false
                    });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
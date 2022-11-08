using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RENT.Api.Configuration;
using RENT.Data.Context;
using RENT.Data.Helpers;
using RENT.Data.Interfaces.IServices;
using RENT.Domain.Entities.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RENT.Services.Services
{
    public class TokenService: ITokenService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;    
        private readonly JwtConfig _jwtConfig;
        public TokenService(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,    
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _jwtConfig = optionsMonitor.CurrentValue; 
        }
        public RefreshToken GetRefreshToken(SecurityToken token, string rand, ApplicationUser user)
        {
            RefreshToken refreshToken = new()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = token.ValidFrom,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Expires = token.ValidTo,
                IsRevoked = false,
                Token = rand
            };
            return refreshToken;
        }

        public async Task<AuthResult> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (user?.Email == null) throw new ArgumentNullException();
            var roleClaims = new List<Claim>();
            roles.ToList().ForEach(role =>
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            });

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("guid", user.Id.ToString()),
                }.Union(roleClaims)),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var rand = RandomString.RandString(25) + Guid.NewGuid();
            var refreshToken = GetRefreshToken(token, rand, user);

            _context.RefreshToken.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResult> VerifyToken(TokenRequests tokenRequest, ClaimsPrincipal principal, SecurityToken validatedToken)
        {
            // This validation function will make sure that the token meets the validation parameters
            // and its an actual jwt token not just a random string
            // Now we need to check if the token has a valid security algorithm

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                if (result == false)
                {
                    return null;
                }
            }
            // Will get the time stamp in unix time
            var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            // we convert the expiry date from seconds to the date
            var expDate = UnixTimeStamp.UnixTimeStampToDateTime(utcExpiryDate);

            if (expDate > DateTime.UtcNow)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "We cannot refresh this since the token has not expired" },
                    Success = false
                };
            }

            // Check the token we got if its saved in the db
            var storedRefreshToken = await _context.RefreshToken.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "refresh token doesn't exist" },
                    Success = false
                };
            }

            // Check the date of the saved token if it has expired
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "token has expired, user needs to reloading" },
                    Success = false
                };
            }

            // check if the refresh token has been used
            if (storedRefreshToken.IsUsed)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "token has been used" },
                    Success = false
                };
            }

            // Check if the token is revoked
            if (storedRefreshToken.IsRevoked)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "token has been revoked" },
                    Success = false
                };
            }

            // we are getting here the jwt token id
            var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            // check the id that the received token has against the id saved in the db
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "the token doesn't match the saved token" },
                    Success = false
                };
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshToken.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId.ToString());
            return await GenerateJwtTokenAsync(dbUser);
        }
    }
}

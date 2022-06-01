namespace RENT.Data.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailPasswordService _mail;
        private readonly JwtConfig _jwtConfig;

        public UserRepository(AppDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IEmailPasswordService mail,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mail = mail ?? throw new ArgumentNullException(nameof(mail));
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        public async Task AddUserAsync(UserRegistrationDto user)
        {
            if (user.Roles == "User")
            {
                Seller seller = _mapper.Map<Seller>(user);
                _context.Seller.Add(seller);
                Address addres = new()
                {
                    SellerId = seller.Id
                };
                _context.Address.Add(addres);
                //Address addres = new()
                //{
                //    SellerId = seller.Id
                //};

                //_context.Address.Add(addres);

                await _context.SaveChangesAsync();
            }
            if (user.Roles == "Client")
            {
                Customers customers = _mapper.Map<Customers>(user);
                _context.Add(customers);
                Address addres = new()
                {
                    CustomerId = customers.Id
                };
                _context.Address.Add(addres);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Temp>> GetUserInfo(ApplicationUser user, AuthResult token, string ImageSrc)
        {
            var role = await _userManager.GetRolesAsync(user);

            foreach (var item in role)
            {
                switch (item)
                {
                    case "User":
                        var seller = await _context.Seller
                        .Include(address => address.Address)
                        .Where(u => u.UserId == new Guid(user.Id.ToString()))
                        .ToListAsync();

                        List<Temp> selerDto = _mapper.Map<List<Temp>>(seller);

                        selerDto.Where(t => t.Token == null).ToList().ForEach(t => t.Token = token.Token);
                        selerDto.Where(t => t.RefreshToken == null).ToList().ForEach(t => t.RefreshToken = token.RefreshToken);

                        foreach (var image in selerDto)
                        {
                            string[] imagesNames = image.ImageName.Split(',');
                            foreach (var img in imagesNames)
                            {
                                image.ImageSrc.Add(string.Format("{0}/Images/{1}", ImageSrc, img));
                            }
                        }

                        //    }

                        //}

                        return selerDto;

                        throw new Exception("User does not exist");

                        //case "Client":
                        //    var client = await _context.Customers
                        //        .Include(address => address.Address)
                        //        .Where(u => u.UserId == new Guid(user.Id.ToString()))
                        //        .ToListAsync();
                        //    var clientDto = _mapper.Map<List<UserInformationDto>>(client);

                        //    clientDto.Where(t => t.Token == null).ToList().ForEach(t => t.Token = token.Token);
                        //    clientDto.Where(t => t.RefreshToken == null).ToList().ForEach(t => t.RefreshToken = token.RefreshToken);

                        //    foreach (var image in clientDto)
                        //    {
                        //        string[] imagesNames = image.ImageName.Split(',');
                        //        foreach (var imgname in imagesNames)
                        //        {
                        //            image.ImageSrc.Add(string.Format("{0}/Images/{1}", ImageSrc, imgname));
                        //        }
                        //    }
                        //    return clientDto;

                        throw new ArgumentException("User does not exist");

                    default:
                        throw new Exception();
                }
            }

            throw new ArgumentException("Can not get data from DB. Role dose not exisit");
        }

        public async Task<bool> NewPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return false;
            }
            else if (user.ResetToken != model.Token
                && user.ResetTokenExpires < DateTime.UtcNow)
            {
                return false;
            }
            else
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (resetPassResult.Succeeded)
                {
                    user.PasswordReset = DateTime.UtcNow;
                    user.ResetToken = null;
                    user.ResetTokenExpires = null;
                    await _userManager.UpdateAsync(user);

                    return true;
                }
            }
            throw new Exception();
        }

        public async Task<bool> SendEmailPasswordReset(ForgotPassword model, string origin, string token)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (token != null)
            {
                user.ResetToken = token;
                user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                throw new Exception();
            }
            var link = $"{origin}/api/Auth/NewPassword?token={token}&email={user.Email}";
            bool sendEmail = _mail.SendEmailPasswordReset(model, link);
            return sendEmail;
        }

        public RefreshToken GetrefreshToken(SecurityToken token, string rand, ApplicationUser user)
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

            var roleClaims = new List<Claim>();
            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

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

            string rand = RandomString.RandString(25) + Guid.NewGuid();
            var refreshToken = GetrefreshToken(token, rand, user);

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
            JwtSecurityTokenHandler jwtTokenHandler = new();

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
                    Errors = new List<string>() { "refresh token doesnt exist" },
                    Success = false
                };
            }

            // Check the date of the saved token if it has expired
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "token has expired, user needs to relogin" },
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

            // check the id that the recieved token has against the id saved in the db
            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthResult()
                {
                    Errors = new List<string>() { "the token doenst mateched the saved token" },
                    Success = false
                };
            }

            storedRefreshToken.IsUsed = true;
            _context.RefreshToken.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId.ToString());
            return await GenerateJwtTokenAsync(dbUser);
        }

        public string StringRandom()
        {
            string unique = RandomString.RandString(36);

            var existName = _userManager.FindByNameAsync(unique);
            if (existName.Result != null)
            {
                StringRandom();
            }
            else
            {
                return unique;
            }
            throw new Exception();
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RENT.Api.Configuration;
using RENT.Data.Context;
using RENT.Data.Interfaces.IRepositories;
using RENT.Domain.Dtos;
using RENT.Domain.Entities;
using RENT.Domain.Entities.Auth;

namespace RENT.Data.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserRepository(AppDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager
            )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task AddNewUserAsync(UserRegistrationDto user)
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

        public async Task<UserInformationDto> GetUserInfo(ApplicationUser user, AuthResult token, string ImageSrc)
        {
            var role = await _userManager.GetRolesAsync(user);

            foreach (var item in role)
            {
                switch (item)
                {
                    case "User":
                        var seller = await _context.Seller
                        .Include(address => address.Address)
                        .Include(product => product.Products)
                        .Where(u => u.UserId == new Guid(user.Id.ToString()))
                        .FirstOrDefaultAsync();

                        var sellerDto = _mapper.Map<UserInformationDto>(seller);
                        var newAddress = _mapper.Map<AddressDto>(seller.Address);
                        sellerDto.AddressDto = newAddress;

                        sellerDto.Token ??= token.Token;
                        sellerDto.RefreshToken ??= token.Token;

                        if (!String.IsNullOrEmpty(sellerDto.ImageName))
                        {
                           sellerDto.ImageSrc = GetImagesUrl(sellerDto.ImageName, ImageSrc);
                        }
                        return sellerDto;

                    case "Client":
                        var client = await _context.Customers
                            .Include(address => address.Address)
                            .Where(u => u.UserId == new Guid(user.Id.ToString()))
                            .FirstOrDefaultAsync();

                        var clientDto = _mapper.Map<UserInformationDto>(client);
                        var addressNew = _mapper.Map<AddressDto>(client.Address);
                        clientDto.AddressDto = addressNew;

                        clientDto.Token ??= token.Token;
                        clientDto.RefreshToken ??= token.Token;

                        if (!String.IsNullOrEmpty(clientDto.ImageName))
                        {
                            clientDto.ImageSrc = GetImagesUrl(clientDto.ImageName, ImageSrc);
                        }

                        return clientDto;

                    default:
                        throw new Exception();
                }
            }
            throw new ArgumentException("Can not get data from DB. Role dose not exisit");
        }

        private List<string> GetImagesUrl(string ImageName, string imageSrc)
        {
            string[] imagesNames = ImageName.Split(',');
            var newImages = new List<string>();
            foreach (string img in imagesNames)
            {
                if (!String.IsNullOrEmpty(img))
                {
                    newImages.Add(string.Format("{0}/Images/{1}", imageSrc, img));
                }
            }
            return newImages;
        }
    }
}
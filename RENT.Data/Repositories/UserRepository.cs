using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RENT.Api.Configuration;
using RENT.Data.Context;
using RENT.Data.Interfaces.IRepositories;
using RENT.Domain.Dtos;
using RENT.Domain.Entities;
using RENT.Domain.Entities.Auth;

namespace RENT.Data.Repositories
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
            Address address = new();
            switch (user.Roles)
            {
                case "User":
                    var seller = _mapper.Map<Seller>(user);
                    _context.Seller.Add(seller);
                    address.SellerId = seller.Id;
                    _context.Address.Add(address);
                    await _context.SaveChangesAsync();
                    break;

                case "Client":
                    var customers = _mapper.Map<Customers>(user);
                    _context.Add(customers);
                    address.CustomerId = customers.Id;
                    _context.Address.Add(address);
                    await _context.SaveChangesAsync();
                    break;

                default:
                    throw new Exception();
            }
        }

        public async Task<UserInformationDto> GetUserInfo(ApplicationUser user, AuthResult token, string imageSrc)
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
                        if (seller == null) throw new ArgumentNullException("Could not find User", nameof(seller));

                        var sellerDto = _mapper.Map<UserInformationDto>(seller);
                        var newAddress = _mapper.Map<AddressDto>(seller.Address);
                        sellerDto.AddressDto = newAddress;

                        sellerDto.Token ??= token.Token;
                        sellerDto.RefreshToken ??= token.Token;

                        if (!string.IsNullOrEmpty(sellerDto.ImageName))
                        {
                            sellerDto.ImageSrc = GetImagesUrl(sellerDto.ImageName, imageSrc);
                        }
                        return sellerDto;

                    case "Client":
                        var client = await _context.Customers
                            .Include(address => address.Address)
                            .Where(u => u.UserId == new Guid(user.Id.ToString()))
                            .FirstOrDefaultAsync();
                        if (client == null) throw new ArgumentNullException("Could not find Client", nameof(client));

                        var clientDto = _mapper.Map<UserInformationDto>(client);
                        var addressNew = _mapper.Map<AddressDto>(client.Address);
                        clientDto.AddressDto = addressNew;

                        clientDto.Token ??= token.Token;
                        clientDto.RefreshToken ??= token.Token;

                        if (!string.IsNullOrEmpty(clientDto.ImageName))
                        {
                            clientDto.ImageSrc = GetImagesUrl(clientDto.ImageName, imageSrc);
                        }
                        return clientDto;

                    default:
                        throw new Exception();
                }
            }
            throw new ArgumentException("Can not get data from DB. Role dose not exist");
        }

        private List<string> GetImagesUrl(string imageName, string imageSrc)
        {
            var imagesNames = imageName.Split(',');
            var newImages = new List<string>();

            imagesNames.ToList().ForEach(img =>
            {
                if (!string.IsNullOrEmpty(img)) return;
                newImages.Add($"{imageSrc}/ Images /{img}");
            });

            return newImages;
        }
    }
}
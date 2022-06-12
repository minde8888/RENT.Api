using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;
using RENT.Domain.Entities.Auth;

namespace RENT.Data.Repositorys
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public BaseRepository(AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task AddItemAsync(T t, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            t.UserId = new Guid(user.Id.ToString());
            await _context.AddAsync(t);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> GetAllItems()
        {
            var item = await _context.Set<T>()
               .Include(a => a.Address)
               .ToListAsync();

            return _mapper.Map<UserDto>(item);
        }

        public async Task<T> GetItemIdAsync(string Id)
        {
            var itemToMap = await _context.Set<T>().
                Include(t => t.Address).
                SingleOrDefaultAsync(x => x.Id == Guid.Parse(Id));

            return itemToMap;
        }

        public async Task<RequestUserDto> UpdateItemAsync(RequestUserDto userDto)
        {
            var item = _context.Set<T>().
                Include(manager => manager.Address).
                Where(m => m.Id == userDto.Id).FirstOrDefault();

            item.Name = userDto.Name;
            item.Surname = userDto.Surname;
            item.Occupation = userDto.Occupation;
            item.PhoneNumber = userDto.PhoneNumber;
            item.DateUpdated = DateTime.UtcNow;

            if (userDto.ImageName?.Any() ?? false)
            {
                item.ImageName = userDto.ImageName;
            }
            if (userDto.AddressDto != null)
            {
                item.Address.City = userDto.AddressDto.City;
                item.Address.Country = userDto.AddressDto.Country;
                item.Address.Street = userDto.AddressDto.Street;
                item.Address.Zip = userDto.AddressDto.Zip;
                item.Address.CompanyCode = userDto.AddressDto.CompanyCode;
                _context.Entry(item.Address).State = EntityState.Modified;
            }
            else
            {
                Address address = new()
                {
                    City = userDto.AddressDto.City,
                    Country = userDto.AddressDto.Country,
                    Street = userDto.AddressDto.Street,
                    Zip = userDto.AddressDto.Zip,
                    CompanyCode = userDto.AddressDto.CompanyCode,
                };
                _context.Address.Add(address);
            }

            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync();
   
            return _mapper.Map<RequestUserDto>(item);
        }

        public async Task RemoveItemAsync(string Id)
        {
            var item = _context.
                Set<T>().
                Where(x => x.Id == Guid.Parse(Id)).
                FirstOrDefault();

            var user = await _userManager.FindByEmailAsync(item.Email);
            item.IsDeleted = true;
            user.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> Search(string name)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name)
                            || e.Surname.Contains(name));
            }
            return await query.ToListAsync();
        }
    }
}
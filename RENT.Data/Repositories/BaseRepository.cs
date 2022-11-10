using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Interfaces.IRepositories;
using RENT.Domain.Dtos;
using RENT.Domain.Dtos.RequestDto;
using RENT.Domain.Entities;
using RENT.Domain.Entities.Auth;

namespace RENT.Data.Repositories
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
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddItemAsync(T t, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception();

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

        public async Task<UserDto> GetItemIdAsync(string id)
        {
            var itemToMap = await _context.Set<T>().
                Include(t => t.Address).
                SingleOrDefaultAsync(x => x.Id == Guid.Parse(id));

            var result = _mapper.Map<UserDto>(itemToMap);
            var newAddress = _mapper.Map<AddressDto>(itemToMap?.Address);
            result.AddressDto = newAddress;

            return result;
        }

        public async Task<UserDto> UpdateItemAsync(RequestUserDto userDto)
        {
            var item = _context.Set<T>().
                Include(manager => manager.Address).
                Single(m => m.Id == userDto.Id);

            if (item == null || userDto == null) throw new ArgumentNullException("Could not get data form DB or Request", nameof(item));

            item.Name = userDto.Name;
            item.Surname = userDto.Surname;
            item.Occupation = userDto.Occupation;
            item.PhoneNumber = userDto.PhoneNumber;
            if (userDto.ImageName != null)
            {
                item.ImageName = userDto.ImageName;
            }
            item.DateUpdated = DateTime.UtcNow;
            item.Address.City = userDto.City;
            item.Address.Country = userDto.Country;
            item.Address.Street = userDto.Street;
            item.Address.Zip = userDto.Zip;
            item.Address.CompanyCode = userDto.CompanyCode;

            _context.Entry(item.Address).State = EntityState.Modified;
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var newAddress = _mapper.Map<AddressDto>(item.Address);
            var itemReturn = _mapper.Map<UserDto>(item);
            itemReturn.AddressDto = newAddress;

            return itemReturn;
        }

        public async Task RemoveItemAsync(string id)
        {
            var item = _context.
                Set<T>().
                Single(x => x.Id == Guid.Parse(id));
            if (item == null) throw new ArgumentNullException("Could not get value from DB", nameof(item));

            var user = await _userManager.FindByEmailAsync(item.Email);
            if (user == null) throw new Exception();

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
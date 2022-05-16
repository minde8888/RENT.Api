using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RENT.Data.Context;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos;
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

        public async Task AddItemAsync(T t)
        {
            await _context.AddAsync(t);
            await _context.SaveChangesAsync();

        }
        public async Task<BaseEntityDto> GetItemIdAsync(string Id)
        {
            var itemToMap = await _context.Set<T>().
                Include(t => t.Address).
                Where(x => x.Id == Guid.Parse(Id)).
                ToListAsync();

            return _mapper.Map<BaseEntityDto>(itemToMap);
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

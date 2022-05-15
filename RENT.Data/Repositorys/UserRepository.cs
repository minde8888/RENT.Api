using AutoMapper;
using RENT.Domain.Entities;
using RENT.Data.Context;
using RENT.Data.Interfaces;
using RENT.Domain.Dtos.Requests;

namespace RENT.Data.Repositorys
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(
            IMapper mapper,
            AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task AddUserAsync(UserRegistrationDto user)
        {
            if (user.Role == "User")
            {
                Seller seller = _mapper.Map<Seller>(user);
                _context.Seller.Add(seller);
                Address addres = new()
                {
                    SellerId = seller.SellerId
                };
                _context.Address.Add(addres);

                await _context.SaveChangesAsync();
            }
            if (user.Role == "Client")
            {
                Customers customers = _mapper.Map<Customers>(user);
                _context.Add(customers);
                Address addres = new()
                {
                    CustomerId = customers.CustomersId
                };
                _context.Add(addres);

                await _context.SaveChangesAsync();
            }
        }
    }
}
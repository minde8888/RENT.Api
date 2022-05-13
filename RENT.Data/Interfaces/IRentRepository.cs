using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Interfaces
{
    public interface IRentRepository
    {
        public Task AddRentToolAsync(RentDto rent);
        public Task<List<Rent>> GetRentIdAsync(Guid rentId);
        public Task<List<RentDto>> GetAllAsync(string ImageSrc);
        public Task<Rent> UpdateRent(RentDto rentDto);
        public Task RemoveRetedToolAsync(string Id);
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NWCodeFirstMVC.Domain.Contracts;
using NWCodeFirstMVC.Domain.Dto;

using NWCodeFirstMVC.Infrastructure.Repositories;

namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly NWCodeFirstMVC.Domain.Contracts.IGenericRepository<NWCodeFirstMVC.Infrastructure.PgModels.User> _repo;
        private readonly IMapper _mapper;
        private readonly PgNwContext _dc;

        public AdminService(
            NWCodeFirstMVC.Domain.Contracts.IGenericRepository<NWCodeFirstMVC.Infrastructure.PgModels.User> repo,
            IMapper mapper,
            PgNwContext dc)
        {
            _repo = repo;
            _mapper = mapper;
            this._dc = dc;
        }

        public async Task<List<NWCodeFirstMVC.Domain.PocoModels.User>> GetAllAsync()
        {
            var entities = await _repo.GetAllAsync();
            var users = _mapper.Map<List<NWCodeFirstMVC.Domain.PocoModels.User>>(entities);
            return users;
        }

        public async Task<AdminUserDto> CreateAsync(AdminUserDto dto)
        {
            var entity = new NWCodeFirstMVC.Infrastructure.PgModels.User
            {
                Username = dto.Username,
                Password = Guid.NewGuid().ToString(), // placeholder; user logs in via Google
                Admin = dto.Admin,
                Firstname = dto.Firstname,
                Occupation = dto.Occupation ?? string.Empty
            };

            _dc.Users.Add(entity);
            await _dc.SaveChangesAsync();

            dto.Pkid = entity.Pkid;
            return dto;
        }
        public async Task<AdminUserDto> GetByIdAsync(int id)
        {
            var e = await _dc.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Pkid == id);
            if (e == null) return null;

            return new AdminUserDto
            {
                Pkid = e.Pkid,
                Username = e.Username,
                Firstname = e.Firstname,
                Occupation = e.Occupation,
            };
        }

    }
}

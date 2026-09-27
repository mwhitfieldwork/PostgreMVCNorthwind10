using NWCodeFirstMVC.Domain.Dto;
using NWCodeFirstMVC.Domain.PocoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface IAdminService
    {
        Task<List<User>> GetAllAsync();
        Task<AdminUserDto> GetByIdAsync(int id);
        Task<AdminUserDto> CreateAsync(AdminUserDto dto);
    }
}

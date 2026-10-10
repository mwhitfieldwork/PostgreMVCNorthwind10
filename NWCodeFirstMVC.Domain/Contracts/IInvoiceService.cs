using System.Threading.Tasks;
using NWCodeFirstMVC.Domain.Dto;

namespace NWCodeFirstMVC.Domain.Contracts
{
    public interface IInvoiceService
    {
        Task SendInvoiceAsync(string email, List<int> productIds);
    }
}

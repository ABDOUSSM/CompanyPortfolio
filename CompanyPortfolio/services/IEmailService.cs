using System.Threading.Tasks;
using CompanyPortfolio.Models;

namespace CompanyPortfolio.Services
{
    public interface IEmailService
    {
        Task<bool> SendContactEmailAsync(ContactFormModel model);
    }
}
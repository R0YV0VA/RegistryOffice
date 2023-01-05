using RegistryOffice.Domain.Models;

namespace RegistryOffice.Application.Services;

public interface IMarriedService
{
    Task<MarriedModel> GetMarriedById(int Id, string connectionString);
    Task<List<MarriedModel>> GetAllMarrieds(string connectionString);
    Task<MarriedModel> AddMarried(MarriedToAddModel married, string connectionString);
    Task<MarriedModel> UpdateMarried(MarriedModel married, string connectionString);
    Task<MarriedModel> DeleteMarried(int Id, string connectionString);
}

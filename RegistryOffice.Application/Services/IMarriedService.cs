using RegistryOffice.Domain.Models;

namespace RegistryOffice.Application.Services;

public interface IMarriedService
{
    Task<MarriedModel> GetMarriedById(int Id, string connectionString);
    Task<List<MarriedModel>> GetAllMarrieds(string connectionString);
    Task<MarriedModel> AddMarried(MarriedToAddModel married, string MarriageCertificateIMG, string connectionString);
    Task<MarriedModel> UpdateMarried(MarriedModel married, string impPathToDel, string connectionString);
    Task<MarriedModel> DeleteMarried(int Id, string connectionString);
    Task<bool> SaveLog(string ip, string operation, string connectionString);
}

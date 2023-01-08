using RegistryOffice.Domain.Models;

namespace RegistryOffice.Application.Services;

public interface IBornService
{
    Task<BornModel> GetBornById(int Id, string connectionString);
    Task<List<BornModel>> GetAllBorns(string connectionString);
    Task<BornModel> AddBorn(BornToAddModel born, string BirthCertificateIMG, string connectionString);
    Task<BornModel> UpdateBorn(BornModel born, string impPathToDel, string connectionString);
    Task<BornModel> DeleteBorn(int Id, string connectionString);
    Task<bool> SaveLog(string ip, string operation, string connectionString);
}

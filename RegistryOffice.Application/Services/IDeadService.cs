using RegistryOffice.Domain.Models;

namespace RegistryOffice.Application.Services;

public interface IDeadService
{
    Task<DeadModel> GetDeadById(int Id, string connectionString);
    Task<List<DeadModel>> GetAllDeads(string connectionString);
    Task<DeadModel> AddDead(DeadToAddModel dead, string connectionString);
    Task<DeadModel> UpdateDead(DeadModel dead, string connectionString);
    Task<DeadModel> DeleteDead(int Id, string connectionString);
    Task<bool> SaveLog(string ip, string operation, string connectionString);
}

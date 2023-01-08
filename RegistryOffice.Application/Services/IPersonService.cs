using RegistryOffice.Domain.Models;

namespace RegistryOffice.Application.Services;

public interface IPersonService
{
    Task<PersonModel> GetPersonById(int Id, string connectionString);
    Task<List<PersonModel>> GetAllPersons(string connectionString);
    Task<PersonModel> AddPerson(PersonToAddModel person, string PasportIMG, string connectionString);
    Task<PersonModel> UpdatePerson(PersonModel person, string impPathToDel, string connectionString);
    Task<PersonModel> DeletePerson(int Id, string connectionString);
    Task<bool> SaveLog(string ip, string operation, string connectionString);
}

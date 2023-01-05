using Microsoft.AspNetCore.Mvc;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;
using RegistryOffice.Rest.RestModels;

namespace RegistryOffice.Rest.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : Controller
    {
        readonly IPersonService _personService;
        private readonly IConfiguration _configuration;
        public PersonController(IPersonService personService, IConfiguration configuration)
        {
            _personService = personService;
            _configuration = configuration;
        }
        [HttpGet("{Id}")]
        public async Task<PersonEntity> GetPerson(int Id)
        {
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _personService.GetPersonById(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            await _personService.SaveLog(ipAddress, $"Get person by id=({Id})", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new PersonEntity(result.Id, result.FullName, result.DateOfBirthday, result.Address, result.Citizenship, result.Children, result.MaritalStatus, result.PhoneNumber, result.PasportIMG);
        }
        [HttpGet]
        public async Task<List<PersonEntity>> GetAllPersons()
        {
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _personService.GetAllPersons(_configuration.GetConnectionString("PostgreSQLConnectionString"));
            var _persons = new List<PersonEntity>();
            foreach (var person in result)
            {
                _persons.Add(new PersonEntity(person.Id, person.FullName, person.DateOfBirthday, person.Address, person.Citizenship, person.Children, person.MaritalStatus, person.PhoneNumber, person.PasportIMG));
            }
            await _personService.SaveLog(ipAddress, "Get all persons", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return _persons;
        }
        [HttpPost]
        public async Task<PersonEntity> AddPerson([FromForm]PersonToAddModel person, IFormFile Image)
        {
            try
            {
                string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string path = "";
                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "PersonsPasportIMG"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
                person.PasportIMG = Path.Combine(path, Image.FileName);
                var result = await _personService.AddPerson(person, _configuration.GetConnectionString("PostgreSQLConnectionString"));
                await _personService.SaveLog(ipAddress, $"Add person ({person.FullName})", _configuration.GetConnectionString("PostgreSQLConnectionString"));
                return new PersonEntity(result.Id, result.FullName, result.DateOfBirthday, result.Address, result.Citizenship, result.Children, result.MaritalStatus, result.PhoneNumber, result.PasportIMG);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        [HttpPut]
        public async Task<PersonEntity> UpdatePerson([FromForm]PersonModel person, IFormFile Image)
        {
            try
            {
                string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string path = "";
                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "PersonsPasportIMG"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }
                person.PasportIMG = Path.Combine(path, Image.FileName);
                var result = await _personService.UpdatePerson(person, _configuration.GetConnectionString("PostgreSQLConnectionString"));
                await _personService.SaveLog(ipAddress, $"Update person ({person.FullName})", _configuration.GetConnectionString("PostgreSQLConnectionString"));
                return new PersonEntity(result.Id, result.FullName, result.DateOfBirthday, result.Address, result.Citizenship, result.Children, result.MaritalStatus, result.PhoneNumber, result.PasportIMG);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        [HttpDelete("{Id}")]
        public async Task<PersonEntity> DeletePerson(int Id)
        {
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _personService.DeletePerson(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            await _personService.SaveLog(ipAddress, $"Delete person by id=({Id})", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new PersonEntity(result.Id, result.FullName, result.DateOfBirthday, result.Address, result.Citizenship, result.Children, result.MaritalStatus, result.PhoneNumber, result.PasportIMG);
        }
    }
}

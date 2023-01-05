using Microsoft.AspNetCore.Mvc;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;
using RegistryOffice.Rest.RestModels;

namespace RegistryOffice.Rest.Controllers;

[Route("api/deads")]
[ApiController]
public class DeadController : Controller
{
    readonly IDeadService _deadService;
    private readonly IConfiguration _configuration;
    public DeadController(IDeadService deadService, IConfiguration configuration)
    {
        _deadService = deadService;
        _configuration = configuration;
    }
    [HttpGet("{Id}")]
    public async Task<DeadEntity> GetDead(int Id)
    {
        var result = await _deadService.GetDeadById(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
    }
    [HttpGet]
    public async Task<List<DeadEntity>> GetAllDeads()
    {
        var result = await _deadService.GetAllDeads(_configuration.GetConnectionString("PostgreSQLConnectionString"));
        var _deads = new List<DeadEntity>();
        foreach (var dead in result)
        {
            _deads.Add(new DeadEntity(dead.Id, dead.FullName, dead.DeathCaseIMG));
        }
        return _deads;
    }
    [HttpPost]
    public async Task<DeadEntity> AddDead([FromForm] DeadToAddModel dead, IFormFile Image)
    {
        try
        {
            string path = "";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "DeathCaseIMG"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
            dead.DeathCaseIMG = Path.Combine(path, Image.FileName);
            var result = await _deadService.AddDead(dead, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    [HttpPut]
    public async Task<DeadEntity> UpdateDead([FromForm] DeadModel dead, IFormFile Image)
    {
        try
        {
            string path = "";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "DeathCaseIMG"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
            dead.DeathCaseIMG = Path.Combine(path, Image.FileName);
            var result = await _deadService.UpdateDead(dead, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    [HttpDelete("{Id}")]
    public async Task<DeadEntity> DeleteDead(int Id)
    {
        var result = await _deadService.DeleteDead(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
    }
}

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;
using RegistryOffice.Infrastructure.Services;
using RegistryOffice.Rest.RestModels;
using System.Threading;

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
    [EnableCors("AllowAll")]
    [HttpGet("{Id}")]
    public async Task<DeadEntity> GetDead(int Id)
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _deadService.GetDeadById(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        await _deadService.SaveLog(ipAddress, $"Get dead by id={Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
    }
    [EnableCors("AllowAll")]
    [HttpGet]
    public async Task<List<DeadEntity>> GetAllDeads()
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _deadService.GetAllDeads(_configuration.GetConnectionString("PostgreSQLConnectionString"));
        var _deads = new List<DeadEntity>();
        foreach (var dead in result)
        {
            _deads.Add(new DeadEntity(dead.Id, dead.FullName, dead.DeathCaseIMG));
        }
        await _deadService.SaveLog(ipAddress, "Get all deads", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return _deads;
    }
    [EnableCors("AllowAll")]
    [HttpPost]
    public async Task<DeadEntity> AddDead([FromForm] DeadToAddModel dead, IFormFile Image)
    {
        try
        {
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
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
            await _deadService.SaveLog(ipAddress, $"Add dead FullName={result.FullName}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    [EnableCors("AllowAll")]
    [HttpPut]
    public async Task<DeadEntity> UpdateDead([FromForm] DeadModel dead, IFormFile Image)
    {
        try
        {
            var delFile = await _deadService.GetDeadById(dead.Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            var delFilePath = delFile.DeathCaseIMG;
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
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
            var result = await _deadService.UpdateDead(dead, delFilePath, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            await _deadService.SaveLog(ipAddress, $"Update dead FullName={dead.FullName}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    [EnableCors("AllowAll")]
    [HttpDelete("{Id}")]
    public async Task<DeadEntity> DeleteDead(int Id)
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _deadService.DeleteDead(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        await _deadService.SaveLog(ipAddress, $"Delete dead FullName={result.FullName}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new DeadEntity(result.Id, result.FullName, result.DeathCaseIMG);
    }
}

using Microsoft.AspNetCore.Mvc;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;
using RegistryOffice.Rest.RestModels;

namespace RegistryOffice.Rest.Controllers;
[Route("api/borns")]
[ApiController]
public class BornController : Controller
{
    readonly IBornService _bornService;
    private readonly IConfiguration _configuration;
    public BornController(IBornService bornService, IConfiguration configuration)
    {
        _bornService = bornService;
        _configuration = configuration;
    }
    [HttpGet("{Id}")]
    public async Task<BornEntity> GetBorn(int Id)
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _bornService.GetBornById(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        await _bornService.SaveLog(ipAddress, $"Get born by id={Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new BornEntity(result.Id, result.FullName, result.ParentId1, result.ParentId2, result.BirthDate, result.BirthPlace, result.BirthCertificateIMG);
    }
    [HttpGet]
    public async Task<List<BornEntity>> GetAllBorns()
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _bornService.GetAllBorns(_configuration.GetConnectionString("PostgreSQLConnectionString"));
        var _borns = new List<BornEntity>();
        foreach (var born in result)
        {
            _borns.Add(new BornEntity(born.Id, born.FullName, born.ParentId1, born.ParentId2, born.BirthDate, born.BirthPlace, born.BirthCertificateIMG));
        }
        await _bornService.SaveLog(ipAddress, "Get all borns", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return _borns;
    }
    [HttpPost]
    public async Task<BornEntity> AddBorn([FromForm] BornToAddModel born, IFormFile Image)
    {
        try
        {
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string path = "";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "BirthCertificateIMG"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
            born.BirthCertificateIMG = Path.Combine(path, Image.FileName);
            var result = await _bornService.AddBorn(born, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            await _bornService.SaveLog(ipAddress, $"Add born with id={result.Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new BornEntity(result.Id, result.FullName, result.ParentId1, result.ParentId2, result.BirthDate, result.BirthPlace, result.BirthCertificateIMG);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    [HttpPut]
    public async Task<BornEntity> UpdateBorn([FromForm] BornModel born, IFormFile Image)
    {
        try
        {
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string path = "";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "BirthCertificateIMG"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
            born.BirthCertificateIMG = Path.Combine(path, Image.FileName);
            var result = await _bornService.UpdateBorn(born, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            await _bornService.SaveLog(ipAddress, $"Update born with id={result.Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new BornEntity(result.Id, result.FullName, result.ParentId1, result.ParentId2, result.BirthDate, result.BirthPlace, result.BirthCertificateIMG);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    [HttpDelete("{Id}")]
    public async Task<BornEntity> DeleteBorn(int Id)
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _bornService.DeleteBorn(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        await _bornService.SaveLog(ipAddress, $"Delete born with id={Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new BornEntity(result.Id, result.FullName, result.ParentId1, result.ParentId2, result.BirthDate, result.BirthPlace, result.BirthCertificateIMG);
    }
}

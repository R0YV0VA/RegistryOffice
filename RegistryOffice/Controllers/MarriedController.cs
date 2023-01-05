using Microsoft.AspNetCore.Mvc;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;
using RegistryOffice.Rest.RestModels;

namespace RegistryOffice.Rest.Controllers;

[Route("api/marrieds")]
[ApiController]
public class MarriedController : Controller
{
    readonly IMarriedService _marriedService;
    private readonly IConfiguration _configuration;
    public MarriedController(IMarriedService marriedService, IConfiguration configuration)
    {
        _marriedService = marriedService;
        _configuration = configuration;
    }
    [HttpGet("{Id}")]
    public async Task<MarriedEntity> GetMarried(int Id)
    {
        var result = await _marriedService.GetMarriedById(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
    }
    [HttpGet]
    public async Task<List<MarriedEntity>> GetAllMarrieds()
    {
        var result = await _marriedService.GetAllMarrieds(_configuration.GetConnectionString("PostgreSQLConnectionString"));
        var _marrieds = new List<MarriedEntity>();
        foreach (var married in result)
        {
            _marrieds.Add(new MarriedEntity(married.Id, married.Person1Id, married.Person2Id, married.DateOfMarriage, married.MarriageCertificateIMG));
        }
        return _marrieds;
    }
    [HttpPost]
    public async Task<MarriedEntity> AddMarried([FromForm] MarriedToAddModel married, IFormFile Image)
    {
        try
        {
            string path = "";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "MarriageCertificateIMG"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
            married.MarriageCertificateIMG = Path.Combine(path, Image.FileName);
            var result = await _marriedService.AddMarried(married, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
    [HttpPut]
    public async Task<MarriedEntity> UpdateMarried([FromForm] MarriedModel married, IFormFile Image)
    {
        try
        {
            string path = "";
            path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "MarriageCertificateIMG"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Path.Combine(path, Image.FileName), FileMode.Create))
            {
                await Image.CopyToAsync(fileStream);
            }
            married.MarriageCertificateIMG = Path.Combine(path, Image.FileName);
            var result = await _marriedService.UpdateMarried(married, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
    [HttpDelete("{Id}")]
    public async Task<MarriedEntity> DeleteMarried(int Id)
    {
        var result =  await _marriedService.DeleteMarried(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
    }
}
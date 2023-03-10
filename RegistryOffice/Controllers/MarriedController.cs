using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;
using RegistryOffice.Infrastructure.Services;
using RegistryOffice.Rest.RestModels;
using System;

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
    [EnableCors("AllowAll")]
    [HttpGet("{Id}")]
    public async Task<MarriedEntity> GetMarried(int Id)
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _marriedService.GetMarriedById(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        await _marriedService.SaveLog(ipAddress, $"Get married by id={Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
    }
    [EnableCors("AllowAll")]
    [HttpGet]
    public async Task<List<MarriedEntity>> GetAllMarrieds()
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _marriedService.GetAllMarrieds(_configuration.GetConnectionString("PostgreSQLConnectionString"));
        var _marrieds = new List<MarriedEntity>();
        foreach (var married in result)
        {
            _marrieds.Add(new MarriedEntity(married.Id, married.Person1Id, married.Person2Id, married.DateOfMarriage, married.MarriageCertificateIMG));
        }
        await _marriedService.SaveLog(ipAddress, "Get all marrieds", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return _marrieds;
    }
    [EnableCors("AllowAll")]
    [HttpPost]
    public async Task<MarriedEntity> AddMarried([FromForm] MarriedToAddModel married, IFormFile Image)
    {
        try
        {
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
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
            var result = await _marriedService.AddMarried(married, Path.Combine(path, Image.FileName), _configuration.GetConnectionString("PostgreSQLConnectionString"));
            await _marriedService.SaveLog(ipAddress, $"Add married Id={result.Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
    [EnableCors("AllowAll")]
    [HttpPut]
    public async Task<MarriedEntity> UpdateMarried([FromForm]int Id, [FromForm]MarriedToAddModel married, IFormFile Image)
    {
        try
        {
            var delFile = await _marriedService.GetMarriedById(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            var delFilePath = delFile.MarriageCertificateIMG;
            string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
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
            var _married = new MarriedModel { Id = Id, DateOfMarriage = married.DateOfMarriage, Person1Id = married.Person1Id, Person2Id = married.Person2Id, MarriageCertificateIMG = Path.Combine(path, Image.FileName) };
            var result = await _marriedService.UpdateMarried(_married, delFilePath, _configuration.GetConnectionString("PostgreSQLConnectionString"));
            await _marriedService.SaveLog(ipAddress, $"Update married Id={result.Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
            return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
    [EnableCors("AllowAll")]
    [HttpDelete("{Id}")]
    public async Task<MarriedEntity> DeleteMarried(int Id)
    {
        string ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
        var result =  await _marriedService.DeleteMarried(Id, _configuration.GetConnectionString("PostgreSQLConnectionString"));
        await _marriedService.SaveLog(ipAddress, $"Delete married Id={Id}", _configuration.GetConnectionString("PostgreSQLConnectionString"));
        return new MarriedEntity(result.Id, result.Person1Id, result.Person2Id, result.DateOfMarriage, result.MarriageCertificateIMG);
    }
}
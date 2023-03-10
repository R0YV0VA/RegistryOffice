using Npgsql;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;

namespace RegistryOffice.Infrastructure.Services;

public class MarriedService : IMarriedService
{
    private readonly GenerateID generateID = new GenerateID();
    public async Task<MarriedModel> GetMarriedById(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand($"SELECT * FROM marrieds WHERE id=\'{Id}\'", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _married = new MarriedModel();
        while (rdr.Read())
        {
            _married.Id = rdr.GetInt32(0);
            _married.Person1Id = rdr.GetInt32(1);
            _married.Person2Id = rdr.GetInt32(2);
            _married.DateOfMarriage = rdr.GetString(3);
            _married.MarriageCertificateIMG = rdr.GetString(4);
        }
        return _married;
    }
    public async Task<List<MarriedModel>> GetAllMarrieds(string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("SELECT * FROM marrieds", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _marrieds = new List<MarriedModel>();
        while (rdr.Read())
        {
            var _married = new MarriedModel();
            _married.Id = rdr.GetInt32(0);
            _married.Person1Id = rdr.GetInt32(1);
            _married.Person2Id = rdr.GetInt32(2);
            _married.DateOfMarriage = rdr.GetString(3);
            _married.MarriageCertificateIMG = rdr.GetString(4);
            _marrieds.Add(_married);
        }
        return _marrieds;
    }
    public async Task<MarriedModel> AddMarried(MarriedToAddModel married, string MarriageCertificateIMG, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("INSERT INTO marrieds (id, first_person_id, second_person_id, date_of_marriage, marriage_certificate_img) VALUES (@Id, @Person1Id, @Person2Id, @DateOfMarriage, @MarriageCertificateIMG) RETURNING *;", con);
        database.Parameters.AddWithValue("Id", generateID.getID());
        database.Parameters.AddWithValue("Person1Id", married.Person1Id);
        database.Parameters.AddWithValue("Person2Id", married.Person2Id);
        database.Parameters.AddWithValue("DateOfMarriage", married.DateOfMarriage);
        database.Parameters.AddWithValue("MarriageCertificateIMG", MarriageCertificateIMG);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _married = new MarriedModel();
        while (rdr.Read())
        {
            _married.Id = rdr.GetInt32(0);
            _married.Person1Id = rdr.GetInt32(1);
            _married.Person2Id = rdr.GetInt32(2);
            _married.DateOfMarriage = rdr.GetString(3);
            _married.MarriageCertificateIMG = rdr.GetString(4);
        }
        return _married;
    }
    public async Task<MarriedModel> UpdateMarried(MarriedModel married, string impPathToDel, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("UPDATE marrieds SET first_person_id=@Person1Id, second_person_id=@Person2Id, date_of_marriage=@DateOfMarriage, marriage_certificate_img=@MarriageCertificateIMG WHERE id=@Id RETURNING *;", con);
        database.Parameters.AddWithValue("Id", married.Id);
        database.Parameters.AddWithValue("Person1Id", married.Person1Id);
        database.Parameters.AddWithValue("Person2Id", married.Person2Id);
        database.Parameters.AddWithValue("DateOfMarriage", married.DateOfMarriage);
        database.Parameters.AddWithValue("MarriageCertificateIMG", married.MarriageCertificateIMG);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _married = new MarriedModel();
        while (rdr.Read())
        {
            _married.Id = rdr.GetInt32(0);
            _married.Person1Id = rdr.GetInt32(1);
            _married.Person2Id = rdr.GetInt32(2);
            _married.DateOfMarriage = rdr.GetString(3);
            _married.MarriageCertificateIMG = rdr.GetString(4);
        }
        if (File.Exists(impPathToDel))
        {
            File.Delete(impPathToDel);
            return _married;
        }
        else
        {
            Console.WriteLine($"File {impPathToDel} not exist.");
            return _married;
        }
    }
    public async Task<MarriedModel> DeleteMarried(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand($"DELETE FROM marrieds WHERE id=\'{Id}\' RETURNING *", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _married = new MarriedModel();
        while (rdr.Read())
        {
            _married.Id = rdr.GetInt32(0);
            _married.Person1Id = rdr.GetInt32(1);
            _married.Person2Id = rdr.GetInt32(2);
            _married.DateOfMarriage = rdr.GetString(3);
            _married.MarriageCertificateIMG = rdr.GetString(4);
        }
        if (File.Exists(_married.MarriageCertificateIMG))
        {
            File.Delete(_married.MarriageCertificateIMG);
            return _married;
        }
        else
        {
            Console.WriteLine($"File {_married.MarriageCertificateIMG} not exist.");
            _married.MarriageCertificateIMG = "File not exist";
            return _married;
        }
    }
    public async Task<bool> SaveLog(string ip, string operation, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("INSERT INTO logs (id, operation, timestamp) VALUES (@Id, @Operation, @Timestamp);", con);
        database.Parameters.AddWithValue("Id", generateID.getID());
        database.Parameters.AddWithValue("Operation", ip + " - " + operation);
        database.Parameters.AddWithValue("Timestamp", DateTime.UtcNow);
        await database.PrepareAsync();
        await database.ExecuteNonQueryAsync();
        return true;
    }
}

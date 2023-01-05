using Npgsql;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;

namespace RegistryOffice.Infrastructure.Services;

public class MarriedService : IMarriedService
{
    public async Task<MarriedModel> GetMarriedById(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand($"SELECT * FROM marrieds WHERE id=\'{Id}\'", con);
        await using NpgsqlDataReader rdr = database.ExecuteReader();
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
        con.Open();
        using var database = new NpgsqlCommand("SELECT * FROM marrieds", con);
        await using NpgsqlDataReader rdr = database.ExecuteReader();
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
    public async Task<MarriedModel> AddMarried(MarriedToAddModel married, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand("INSERT INTO marrieds (first_person_id, second_person_id, date_of_marriage, marriage_certificate_img) VALUES (@Person1Id, @Person2Id, @DateOfMarriage, @MarriageCertificateIMG) RETURNING *;", con);
        database.Parameters.AddWithValue("Person1Id", married.Person1Id);
        database.Parameters.AddWithValue("Person2Id", married.Person2Id);
        database.Parameters.AddWithValue("DateOfMarriage", married.DateOfMarriage);
        database.Parameters.AddWithValue("MarriageCertificateIMG", married.MarriageCertificateIMG);
        database.Prepare();
        await using NpgsqlDataReader rdr = database.ExecuteReader();
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
    public async Task<MarriedModel> UpdateMarried(MarriedModel married, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand("UPDATE marrieds SET first_person_id=@Person1Id, second_person_id=@Person2Id, date_of_marriage=@DateOfMarriage, marriage_certificate_img=@MarriageCertificateIMG WHERE id=@Id RETURNING *;", con);
        database.Parameters.AddWithValue("Id", married.Id);
        database.Parameters.AddWithValue("Person1Id", married.Person1Id);
        database.Parameters.AddWithValue("Person2Id", married.Person2Id);
        database.Parameters.AddWithValue("DateOfMarriage", married.DateOfMarriage);
        database.Parameters.AddWithValue("MarriageCertificateIMG", married.MarriageCertificateIMG);
        database.Prepare();
        await using NpgsqlDataReader rdr = database.ExecuteReader();
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
    public async Task<MarriedModel> DeleteMarried(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand($"DELETE FROM marrieds WHERE id=\'{Id}\' RETURNING *", con);
        await using NpgsqlDataReader rdr = database.ExecuteReader();
        var _married = new MarriedModel();
        while (rdr.Read())
        {
            _married.Id = rdr.GetInt32(0);
            _married.Person1Id = rdr.GetInt32(1);
            _married.Person2Id = rdr.GetInt32(2);
            _married.DateOfMarriage = rdr.GetString(3);
            _married.MarriageCertificateIMG = rdr.GetString(4);
        }
        File.Delete(_married.MarriageCertificateIMG);
        return _married;
    }
    public async Task<bool> SaveLog(string ip, string operation, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand("INSERT INTO logs (operation, timestamp) VALUES (@Operation, @Timestamp);", con);
        database.Parameters.AddWithValue("Operation", ip + " - " + operation);
        database.Parameters.AddWithValue("Timestamp", DateTime.UtcNow);
        database.Prepare();
        await database.ExecuteNonQueryAsync();
        return true;
    }
}

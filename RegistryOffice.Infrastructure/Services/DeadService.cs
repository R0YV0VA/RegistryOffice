using Npgsql;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;

namespace RegistryOffice.Infrastructure.Services;

public class DeadService : IDeadService
{
    private readonly GenerateID generateID = new GenerateID();
    public async Task<DeadModel> GetDeadById(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand($"SELECT * FROM deads WHERE id=\'{Id}\'", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _dead = new DeadModel();
        while (rdr.Read())
        {
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
        }
        return _dead;
    }
    public async Task<List<DeadModel>> GetAllDeads(string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("SELECT * FROM deads", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _deads = new List<DeadModel>();
        while (rdr.Read())
        {
            var _dead = new DeadModel();
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
            _deads.Add(_dead);
        }
        return _deads;
    }
    public async Task<DeadModel> AddDead(DeadToAddModel dead, string DeathCaseIMG, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("INSERT INTO deads (id, full_name, death_case_img) VALUES (@Id, @FullName, @DeathCaseIMG) RETURNING *;", con);
        database.Parameters.AddWithValue("Id", generateID.getID());
        database.Parameters.AddWithValue("FullName", dead.FullName);
        database.Parameters.AddWithValue("DeathCaseIMG", DeathCaseIMG);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _dead = new DeadModel();
        while (rdr.Read())
        {
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
        }
        return _dead;
    }
    public async Task<DeadModel> UpdateDead(DeadModel dead, string impPathToDel, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("UPDATE deads SET full_name=@FullName, death_case_img=@DeathCaseIMG WHERE id=@Id RETURNING *;", con);
        database.Parameters.AddWithValue("Id", dead.Id);
        database.Parameters.AddWithValue("FullName", dead.FullName);
        database.Parameters.AddWithValue("DeathCaseIMG", dead.DeathCaseIMG);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _dead = new DeadModel();
        while (rdr.Read())
        {
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
        }
        if (File.Exists(impPathToDel))
        {
            File.Delete(impPathToDel);
            return _dead;
        }
        else
        {
            Console.WriteLine($"File {impPathToDel} not exist.");
            return _dead;
        }
    }
    public async Task<DeadModel> DeleteDead(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand($"DELETE FROM deads WHERE id=\'{Id}\' RETURNING *;", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _dead = new DeadModel();
        while (rdr.Read())
        {
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
        }
        if (File.Exists(_dead.DeathCaseIMG))
        {
            File.Delete(_dead.DeathCaseIMG);
            return _dead;
        }
        else
        {
            Console.WriteLine($"File {_dead.DeathCaseIMG} not exist.");
            _dead.DeathCaseIMG = "File not exist";
            return _dead;
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

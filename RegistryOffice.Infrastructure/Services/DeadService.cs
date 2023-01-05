using Npgsql;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;

namespace RegistryOffice.Infrastructure.Services;

public class DeadService : IDeadService
{
    public async Task<DeadModel> GetDeadById(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand($"SELECT * FROM deads WHERE id=\'{Id}\'", con);
        await using NpgsqlDataReader rdr = database.ExecuteReader();
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
        con.Open();
        using var database = new NpgsqlCommand("SELECT * FROM deads", con);
        await using NpgsqlDataReader rdr = database.ExecuteReader();
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
    public async Task<DeadModel> AddDead(DeadToAddModel dead, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand("INSERT INTO deads (full_name, death_case_img) VALUES (@FullName, @DeathCaseIMG) RETURNING *;", con);
        database.Parameters.AddWithValue("FullName", dead.FullName);
        database.Parameters.AddWithValue("DeathCaseIMG", dead.DeathCaseIMG);
        database.Prepare();
        await using NpgsqlDataReader rdr = database.ExecuteReader();
        var _dead = new DeadModel();
        while (rdr.Read())
        {
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
        }
        return _dead;
    }
    public async Task<DeadModel> UpdateDead(DeadModel dead, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand("UPDATE deads SET full_name=@FullName, death_case_img=@DeathCaseIMG WHERE id=@Id RETURNING *;", con);
        database.Parameters.AddWithValue("Id", dead.Id);
        database.Parameters.AddWithValue("FullName", dead.FullName);
        database.Parameters.AddWithValue("DeathCaseIMG", dead.DeathCaseIMG);
        database.Prepare();
        await using NpgsqlDataReader rdr = database.ExecuteReader();
        var _dead = new DeadModel();
        while (rdr.Read())
        {
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
        }
        return _dead;
    }
    public async Task<DeadModel> DeleteDead(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        con.Open();
        using var database = new NpgsqlCommand($"DELETE FROM deads WHERE id=\'{Id}\' RETURNING *;", con);
        await using NpgsqlDataReader rdr = database.ExecuteReader();
        var _dead = new DeadModel();
        while (rdr.Read())
        {
            _dead.Id = rdr.GetInt32(0);
            _dead.FullName = rdr.GetString(1);
            _dead.DeathCaseIMG = rdr.GetString(2);
        }
        File.Delete(_dead.DeathCaseIMG);
        return _dead;
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

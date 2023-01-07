using Npgsql;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;

namespace RegistryOffice.Infrastructure.Services;

public class BornService : IBornService
{
    public async Task<BornModel> GetBornById(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var command = new NpgsqlCommand($"SELECT * FROM borns WHERE id=\'{Id}\'", con);
        using var rdr = await command.ExecuteReaderAsync();
        var _born = new BornModel();
        while (await rdr.ReadAsync())
        {
            _born.Id = rdr.GetInt32(0);
            _born.FullName = rdr.GetString(1);
            _born.ParentId1 = rdr.GetInt32(2);
            _born.ParentId2 = rdr.GetInt32(3);
            _born.BirthDate = rdr.GetString(4);
            _born.BirthPlace = rdr.GetString(5);
            _born.BirthCertificateIMG = rdr.GetString(6);
        }
        return _born;
    }

    public async Task<List<BornModel>> GetAllBorns(string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var command = new NpgsqlCommand("SELECT * FROM borns", con);
        using var rdr = await command.ExecuteReaderAsync();
        var borns = new List<BornModel>();
        while (await rdr.ReadAsync())
        {
            var _born = new BornModel();
            _born.Id = rdr.GetInt32(0);
            _born.FullName = rdr.GetString(1);
            _born.ParentId1 = rdr.GetInt32(2);
            _born.ParentId2 = rdr.GetInt32(3);
            _born.BirthDate = rdr.GetString(4);
            _born.BirthPlace = rdr.GetString(5);
            _born.BirthCertificateIMG = rdr.GetString(6);
            borns.Add(_born);
        }
        return borns;
    }
    public async Task<BornModel> AddBorn(BornToAddModel born, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("INSERT INTO marrieds (full_name, first_parent_id, second_parent_id, birth_date, birth_place, birth_certificate_img) VALUES (@FullName, @ParentId1, @ParentId2, @BirthDate, @BirthPlace, @BirthCertificateIMG) RETURNING *;", con);
        database.Parameters.AddWithValue("FullName", born.FullName);
        database.Parameters.AddWithValue("ParentId1", born.ParentId1);
        database.Parameters.AddWithValue("ParentId2", born.ParentId2);
        database.Parameters.AddWithValue("BirthDate", born.BirthDate);
        database.Parameters.AddWithValue("BirthPlace", born.BirthPlace);
        database.Parameters.AddWithValue("BirthCertificateIMG", born.BirthCertificateIMG);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _born = new BornModel();
        while (rdr.Read())
        {
            _born.Id = rdr.GetInt32(0);
            _born.FullName = rdr.GetString(1);
            _born.ParentId1 = rdr.GetInt32(2);
            _born.ParentId2 = rdr.GetInt32(3);
            _born.BirthDate = rdr.GetString(4);
            _born.BirthPlace = rdr.GetString(5);
            _born.BirthCertificateIMG = rdr.GetString(6);
        }
        return _born;
    }
    public async Task<BornModel> UpdateBorn(BornModel born, string impPathToDel, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("UPDATE borns SET full_name=@FullName, first_parent_id=@ParentId1, second_parent_id=@ParentId2, birth_date=@BirthDate, birth_place=@BirthPlace, birth_certificate_img=@BirthCertificateIMG WHERE id=@Id RETURNING *;", con);
        database.Parameters.AddWithValue("FullName", born.FullName);
        database.Parameters.AddWithValue("ParentId1", born.ParentId1);
        database.Parameters.AddWithValue("ParentId2", born.ParentId2);
        database.Parameters.AddWithValue("BirthDate", born.BirthDate);
        database.Parameters.AddWithValue("BirthPlace", born.BirthPlace);
        database.Parameters.AddWithValue("BirthCertificateIMG", born.BirthCertificateIMG);
        database.Parameters.AddWithValue("Id", born.Id);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _born = new BornModel();
        while (rdr.Read())
        {
            _born.Id = rdr.GetInt32(0);
            _born.FullName = rdr.GetString(1);
            _born.ParentId1 = rdr.GetInt32(2);
            _born.ParentId2 = rdr.GetInt32(3);
            _born.BirthDate = rdr.GetString(4);
            _born.BirthPlace = rdr.GetString(5);
            _born.BirthCertificateIMG = rdr.GetString(6);
        }
        try
        {
            File.Delete(impPathToDel);
            return _born;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public async Task<BornModel> DeleteBorn(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("DELETE FROM borns WHERE id=@Id RETURNING *;", con);
        database.Parameters.AddWithValue("Id", Id);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _born = new BornModel();
        while (rdr.Read())
        {
            _born.Id = rdr.GetInt32(0);
            _born.FullName = rdr.GetString(1);
            _born.ParentId1 = rdr.GetInt32(2);
            _born.ParentId2 = rdr.GetInt32(3);
            _born.BirthDate = rdr.GetString(4);
            _born.BirthPlace = rdr.GetString(5);
            _born.BirthCertificateIMG = rdr.GetString(6);
        }
        try
        {
            File.Delete(_born.BirthCertificateIMG);
            return _born;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public async Task<bool> SaveLog(string ip, string operation, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("INSERT INTO logs (operation, timestamp) VALUES (@Operation, @Timestamp);", con);
        database.Parameters.AddWithValue("Operation", ip + " - " + operation);
        database.Parameters.AddWithValue("Timestamp", DateTime.UtcNow);
        await database.PrepareAsync();
        await database.ExecuteNonQueryAsync();
        return true;
    }

}

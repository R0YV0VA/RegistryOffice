using Npgsql;
using RegistryOffice.Application.Services;
using RegistryOffice.Domain.Models;
using System;
using System.Net.Http;

namespace RegistryOffice.Infrastructure.Services;

public class PersonService : IPersonService
{
    public async Task<PersonModel> GetPersonById(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand($"SELECT * FROM persons WHERE id=\'{Id}\'", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _person = new PersonModel();
        while (rdr.Read())
        {
            _person.Id = rdr.GetInt32(0);
            _person.FullName = rdr.GetString(1);
            _person.Address = rdr.GetString(2);
            _person.Citizenship = rdr.GetString(3);
            _person.Children = rdr.GetInt32(4);
            _person.MaritalStatus = rdr.GetBoolean(5);
            _person.PhoneNumber = rdr.GetString(6);
            _person.PasportIMG = rdr.GetString(7);
            _person.DateOfBirthday = rdr.GetString(8);
        }
        return _person;
    }
    public async Task<List<PersonModel>> GetAllPersons(string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("SELECT * FROM persons", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _persons = new List<PersonModel>();
        while (rdr.Read())
        {
            var _person = new PersonModel();
            _person.Id = rdr.GetInt32(0);
            _person.FullName = rdr.GetString(1);
            _person.Address = rdr.GetString(2);
            _person.Citizenship = rdr.GetString(3);
            _person.Children = rdr.GetInt32(4);
            _person.MaritalStatus = rdr.GetBoolean(5);
            _person.PhoneNumber = rdr.GetString(6);
            _person.PasportIMG = rdr.GetString(7);
            _person.DateOfBirthday = rdr.GetString(8);
            _persons.Add(_person);
        }
        return _persons;
    }
    public async Task<PersonModel> AddPerson(PersonToAddModel person, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("INSERT INTO persons (full_name, address, citizenship, children, marital_status, phone_number, pasport_img, date_of_birthday) VALUES (@FullName, @Address, @Citizenship, @Children, @MaritalStatus, @PhoneNumber, @PasportIMG, @DateOfBirthday) RETURNING *;", con);
        database.Parameters.AddWithValue("FullName", person.FullName);       
        database.Parameters.AddWithValue("Address", person.Address);
        database.Parameters.AddWithValue("Citizenship", person.Citizenship);
        database.Parameters.AddWithValue("Children", person.Children);
        database.Parameters.AddWithValue("MaritalStatus", person.MaritalStatus);
        database.Parameters.AddWithValue("PhoneNumber", person.PhoneNumber);
        database.Parameters.AddWithValue("PasportIMG", person.PasportIMG);
        database.Parameters.AddWithValue("DateOfBirthday", person.DateOfBirthday);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _person = new PersonModel();
        while (rdr.Read())
        {
            _person.Id = rdr.GetInt32(0);
            _person.FullName = rdr.GetString(1);
            _person.Address = rdr.GetString(2);
            _person.Citizenship = rdr.GetString(3);
            _person.Children = rdr.GetInt32(4);
            _person.MaritalStatus = rdr.GetBoolean(5);
            _person.PhoneNumber = rdr.GetString(6);
            _person.PasportIMG = rdr.GetString(7);
            _person.DateOfBirthday = rdr.GetString(8);
        }
        return _person;
    }
    public async Task<PersonModel> UpdatePerson(PersonModel person, string impPathToDel, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand("UPDATE persons SET full_name = @full_name, address = @address, citizenship = @citizenship, children = @children, marital_status = @marital_status, phone_number = @phone_number, pasport_img = @pasport_img, date_of_birthday = @date_of_birthday WHERE id=@id RETURNING *;", con);
        database.Parameters.AddWithValue("full_name", person.FullName);
        database.Parameters.AddWithValue("address", person.Address);
        database.Parameters.AddWithValue("citizenship", person.Citizenship);
        database.Parameters.AddWithValue("children", person.Children);
        database.Parameters.AddWithValue("marital_status", person.MaritalStatus);
        database.Parameters.AddWithValue("phone_number", person.PhoneNumber);
        database.Parameters.AddWithValue("pasport_img", person.PasportIMG);
        database.Parameters.AddWithValue("date_of_birthday", person.DateOfBirthday);
        database.Parameters.AddWithValue("id", person.Id);
        await database.PrepareAsync();
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _person = new PersonModel();
        while (rdr.Read())
        {
            _person.Id = rdr.GetInt32(0);
            _person.FullName = rdr.GetString(1);
            _person.Address = rdr.GetString(2);
            _person.Citizenship = rdr.GetString(3);
            _person.Children = rdr.GetInt32(4);
            _person.MaritalStatus = rdr.GetBoolean(5);
            _person.PhoneNumber = rdr.GetString(6);
            _person.PasportIMG = rdr.GetString(7);
            _person.DateOfBirthday = rdr.GetString(8);
        }
        try
        {
            File.Delete(impPathToDel);
            return _person;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public async Task<PersonModel> DeletePerson(int Id, string connectionString)
    {
        using var con = new NpgsqlConnection(connectionString);
        await con.OpenAsync();
        using var database = new NpgsqlCommand($"DELETE FROM persons WHERE id={Id} RETURNING *;", con);
        using NpgsqlDataReader rdr = await database.ExecuteReaderAsync();
        var _person = new PersonModel();
        while (rdr.Read())
        {
            _person.Id = rdr.GetInt32(0);
            _person.FullName = rdr.GetString(1);
            _person.Address = rdr.GetString(2);
            _person.Citizenship = rdr.GetString(3);
            _person.Children = rdr.GetInt32(4);
            _person.MaritalStatus = rdr.GetBoolean(5);
            _person.PhoneNumber = rdr.GetString(6);
            _person.PasportIMG = rdr.GetString(7);
            _person.DateOfBirthday = rdr.GetString(8);
        }
        try
        {
            File.Delete(_person.PasportIMG);
            return _person;
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

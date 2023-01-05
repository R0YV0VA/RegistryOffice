namespace RegistryOffice.Domain.Models;

public class PersonModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string DateOfBirthday { get; set; }
    public string Address { get; set; }
    public string Citizenship { get; set; }
    public int Children { get; set; }
    public bool MaritalStatus { get; set; }
    public string PhoneNumber { get; set; }
    public string PasportIMG { get; set; }
}

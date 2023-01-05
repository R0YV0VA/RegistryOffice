namespace RegistryOffice.Domain.Models;

public class MarriedModel
{
    public int Id { get; set; }
    public int Person1Id { get; set; }
    public int Person2Id { get; set; }
    public string DateOfMarriage { get; set; }
    public string MarriageCertificateIMG { get; set; } = "";
}

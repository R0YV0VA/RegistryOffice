namespace RegistryOffice.Domain.Models;

public class BornToAddModel
{
    public string FullName { get; set; }
    public int ParentId1 { get; set; }
    public int ParentId2 { get; set; }
    public string BirthDate { get; set; }
    public string BirthPlace { get; set; }
    //public string BirthCertificateIMG { get; set; }
}

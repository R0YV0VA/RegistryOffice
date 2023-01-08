namespace RegistryOffice.Infrastructure.Services;

public class GenerateID
{
    public int getID()
    {
        Guid guid = Guid.NewGuid();
        int id = guid.GetHashCode();
        if (id < 0)
        {
            id = -id;
        }
        return id;
    }
}

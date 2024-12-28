using Entities;

public class Location:IEntity
{
    public int Id { get; set; }  
    public double Latitude { get; set; }  // Enlem bilgisi
    public double Longitude { get; set; }  // Boylam bilgisi
    public string? Address { get; set; }  
}

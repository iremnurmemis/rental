
namespace Entities
{
    public class DriverLicence:IEntity
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public string LicenceNo { get; set; }
        public DateTime ValidUntil { get; set; } 
        public DateTime UploadDate { get; set; }
    }
}


namespace Entities
{
    public class UserDetailDto
    {
        public int userId {  get; set; }
        public string fullname { get; set; }
        public string email {  get; set; }
        public string phone {  get; set; }
        public  bool status {  get; set; }
        public bool ısLicenceVerified {  get; set; }
        public decimal balance {  get; set; }
        public string? LicenceNo { get; set; }
        public DateTime? ValidUntil { get; set; }
        public DateTime? UploadDate { get; set; }

    }
}

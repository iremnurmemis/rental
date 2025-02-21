
using Entities;

namespace Core
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; } = true;
        public string? Token { get; set; } //token email gönderme işlemleri içindir

        public bool IsDrivingLicenseVerified {  get; set; }
        public string? TCKN { get; set; }
        public int OperationClaimId { get; set; } //sahip olduğu rol
       
       
    }
}

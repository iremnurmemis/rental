
namespace Entities
{
    public class Notification:IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } //gönderilen kişinin
        public string Message {  get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;
        public bool IsRead {  get; set; }= false;
    }
}

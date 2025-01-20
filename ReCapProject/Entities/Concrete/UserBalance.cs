
namespace Entities
{
    public class UserBalance:IEntity
    {
       public int Id { get; set; }
       public int UserId { get; set; }
       public decimal Balance { get; set; }
       public DateTime LastUpdated { get; set; }
       
    }
}

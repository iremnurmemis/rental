
namespace Entities
{
    public class BalancePackage:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price {  get; set; }
        public decimal CreditAmount {  get; set; }
    }
}

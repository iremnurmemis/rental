
using Core;

namespace Entities
{
    public class LoadBalanceDto:IDto
    {
        public int UserId { get; set; }
        public int CardId { get; set; }
        public int PackageId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.DashboardDtos
{
    public class PaymentStatisticsDto
    {
        public string Period { get; set; } 
        public decimal TotalRevenue { get; set; } 
    }
}

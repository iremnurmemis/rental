using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.DashboardDtos
{
    public class RentalStatisticsDto
    {
        public string Label { get; set; } // Gün, Ay,Yıl
        public int Count { get; set; } 
    }
}

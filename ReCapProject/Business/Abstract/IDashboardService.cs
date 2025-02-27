using Entities.DTOs.DashboardDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IDashboardService
    {
        List<CategoryRentalStatsDto> GetRentedCarsByCategory();
        List<RentalStatisticsDto> GetRentalStatistics(string filter);
        CarAvailabilityDto GetCarAvailability();
        List<PaymentStatisticsDto> GetRevenueStatistics(string filter);
    }
}

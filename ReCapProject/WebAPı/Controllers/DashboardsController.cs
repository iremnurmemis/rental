using Business;
using Business.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardsController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("rented-cars-by-category")]
        public IActionResult GetRentedCarsByCategory()
        {
            var result = _dashboardService.GetRentedCarsByCategory();
            return Ok(result);
        }

        [HttpGet("rental-count-by-filter")]
        public IActionResult GetRentalStatistics([FromQuery] string filter)
        {
            var result = _dashboardService.GetRentalStatistics(filter);
            return Ok(result);
        }

        [HttpGet("availability")]
        public IActionResult GetCarAvailability()
        {
            var result = _dashboardService.GetCarAvailability();
            return Ok(result);
        }

        [HttpGet("revenue")]
        public IActionResult GetRevenueStatistics([FromQuery] string filter)
        {
            if (filter != "daily" && filter != "monthly" && filter != "yearly")
                return BadRequest("Geçersiz filtre. 'daily', 'monthly' veya 'yearly' olmalıdır.");

            var result = _dashboardService.GetRevenueStatistics(filter);
            return Ok(result);
        }

    }
}

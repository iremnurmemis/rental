using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.DashboardDtos
{
    public class CarAvailabilityDto
    {
        public int AvailableCars { get; set; }
        public int RentedCars { get; set; }
    }
}

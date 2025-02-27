using Business.Abstract;
using DataAccess;
using DataAccess.Migrations;
using Entities.DTOs.DashboardDtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class DashboardManager : IDashboardService
    {
        private readonly ICarRentalDal _carRentalDal;
        private readonly ICategoryDal _categoryDal;
        private readonly ICarDal _carDal;
        private readonly IPaymentDal _paymentDal;

        public DashboardManager(ICarRentalDal carRentalDal, ICategoryDal categoryDal, ICarDal carDal, IPaymentDal paymentDal)
        {
            _carRentalDal = carRentalDal;
            _categoryDal = categoryDal;
            _carDal = carDal;
            _paymentDal = paymentDal;
        }

        public List<CategoryRentalStatsDto> GetRentedCarsByCategory()
        {
            var allCategories = new List<string> { "BASIC", "EASY", "COMFORT", "COOL", "ELECTRIC" };

            var rentals = _carRentalDal.GetRentalsWithCar();
            var categories = _categoryDal.GetAll();

            var categoryRentalCounts = rentals.GroupBy(r => r.Car.CategoryId).Select(g => new CategoryRentalStatsDto
            {
                CategoryName = categories.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Bilinmeyen",
                RentalCount = g.Count(),
            }).ToList();

            var result = allCategories.Select(cat => new CategoryRentalStatsDto
            {
                CategoryName = cat,
                RentalCount = categoryRentalCounts.FirstOrDefault(r => r.CategoryName == cat)?.RentalCount ?? 0
            }).ToList();

            return result;
        }
        public List<RentalStatisticsDto> GetRentalStatistics(string filter)
        {
            var now = DateTime.UtcNow;
            var rentals = _carRentalDal.GetAll();

            List<RentalStatisticsDto> result = new List<RentalStatisticsDto>();

            if (filter == "daily")
            {
                result = rentals
                         .Where(r => r.StartDate.Date == now.Date)
                         .GroupBy(r => r.StartDate.Hour)
                         .Select(g => new RentalStatisticsDto { Label = $"{g.Key}:00", Count = g.Count() })
                         .OrderBy(r => r.Label) // Saat sıralaması
                         .ToList();

            }
            else if (filter == "weekly")
            {
                var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek + 1); // Pazartesi başlangıç
                result = rentals
                        .Where(r => r.StartDate.Date >= startOfWeek)
                        .GroupBy(r => r.StartDate.DayOfWeek)
                        .Select(g => new RentalStatisticsDto { Label = g.Key.ToString(), Count = g.Count() })
                        .OrderBy(r => (int)Enum.Parse(typeof(DayOfWeek), r.Label)) // Pazartesi'den itibaren sırala
                        .ToList();

            }
            else if (filter == "monthly")
            {
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                result = rentals
                         .Where(r => r.StartDate.Date >= startOfMonth)
                         .GroupBy(r => r.StartDate.Day)
                         .Select(g => new RentalStatisticsDto { Label = $"Gün {g.Key}", Count = g.Count() })
                         .OrderBy(r => int.Parse(r.Label.Split(' ')[1])) // Gün sıralaması
                         .ToList();

            }
            else if (filter == "yearly")
            {
                var startOfYear = new DateTime(now.Year, 1, 1);
                result = rentals
                        .Where(r => r.StartDate.Date >= startOfYear)
                        .GroupBy(r => r.StartDate.Month)
                        .Select(g => new RentalStatisticsDto { Label = new DateTime(2024, g.Key, 1).ToString("MMMM"), Count = g.Count() })
                        .OrderBy(r => DateTime.ParseExact(r.Label, "MMMM", new CultureInfo("tr-TR")).Month) // Ocak → Aralık sırası
                        .ToList();

            }

            return result;
        }

        public CarAvailabilityDto GetCarAvailability()
        {
            var totalCars = _carDal.GetAll().Count();
            var rentedCars = _carRentalDal.GetAll().Count(r => r.RentalStatus == RentalStatus.Active);
            var availableCars = totalCars - rentedCars;

            return new CarAvailabilityDto
            {
                AvailableCars = availableCars,
                RentedCars = rentedCars
            };

        }

        public List<PaymentStatisticsDto> GetRevenueStatistics(string filter)
        {
            var payments=_paymentDal.GetAll();
            var query = payments.AsQueryable();

            if (filter == "daily")
            {
                return query
                    .Where(p => p.CreatedTime >= DateTime.UtcNow.AddDays(-30)) // Son 30 gün
                    .GroupBy(p => p.CreatedTime.Date)
                    .Select(g => new PaymentStatisticsDto
                    {
                        Period = g.Key.ToString("yyyy-MM-dd"),
                        TotalRevenue = g.Sum(p => p.TotalPrice)
                    })
                    .OrderBy(p => p.Period)
                    .ToList();
            }
            else if (filter == "monthly")
            {
                return query
                    .Where(p => p.CreatedTime >= DateTime.UtcNow.AddMonths(-12)) // Son 12 ay
                    .GroupBy(p => new { p.CreatedTime.Year, p.CreatedTime.Month })
                    .Select(g => new PaymentStatisticsDto
                    {
                        Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                        TotalRevenue = g.Sum(p => p.TotalPrice)
                    })
                    .OrderBy(p => p.Period)
                    .ToList();
            }
            else if (filter == "yearly")
            {
                return query
                    .Where(p => p.CreatedTime >= DateTime.UtcNow.AddYears(-2)) 
                    .GroupBy(p => p.CreatedTime.Year)
                    .Select(g => new PaymentStatisticsDto
                    {
                        Period = g.Key.ToString(),
                        TotalRevenue = g.Sum(p => p.TotalPrice)
                    })
                    .OrderBy(p => p.Period)
                    .ToList();
            }

            return new List<PaymentStatisticsDto>();


        }
    }
}

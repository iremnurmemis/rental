
using Entities;
using FluentValidation;

namespace Business
{
    public class CarValidator:AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(c => c.PricePerHour).GreaterThan(0);
        }
    }
}

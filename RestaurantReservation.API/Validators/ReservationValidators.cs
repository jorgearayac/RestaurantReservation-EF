using FluentValidation;
using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Validators;

public class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(r => r.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required.");

        RuleFor(r => r.RestaurantId)
            .NotEmpty()
            .WithMessage("Restaurant ID is required.");

        RuleFor(r => r.TableId)
            .NotEmpty()
            .WithMessage("Table ID is required.");

        RuleFor(r => r.ReservationDate)
            .GreaterThan(DateTime.Now)
            .WithMessage("Reservation date must be in the future.");
        
        RuleFor(r => r.PartySize)
            .GreaterThan(0)
            .WithMessage("Party size must be greater than zero.");
    }
}

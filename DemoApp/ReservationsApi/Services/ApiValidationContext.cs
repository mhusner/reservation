using FluentValidation.Results;
using ReservationsApi.Exceptions;

namespace ReservationsApi.Services;

public class ApiValidationContext
{
    private readonly List<ValidationFailure> _failures = new();

    public void EnsureContextIsValid()
    {
        if (_failures.Count > 0)
        {
            throw new ApiValidationException();
        }
    }

    public void AddFailure(ValidationFailure failure)
    {
        _failures.Add(failure);
    }

    public void AddFailures(List<ValidationFailure> failures)
    {
        _failures.AddRange(failures);
    }

    public List<ValidationFailure> GetFailures()
    {
        return _failures.ToList();
    }
}
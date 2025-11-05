using CustomerManagement.Core.Requests.Customer;
using FluentValidation;

namespace CustomerManagement.Domain.Services.Validators;

public class CustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    //private readonly IPersonRepository _repository;
    //private readonly IStudyRepository _studyRepository;

    public CustomerValidator()
    {
        //_repository = repository;
        //_studyRepository = studyRepository;

        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("The person Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name of the person is required.");

        RuleFor(x => x.Address)
            .MaximumLength(75)
            .WithMessage("The short name of the person is limited to 75 characters.");

        //RuleFor(x => x)
        //     .MustAsync(async (request, cancellation) => await Exists(request))
        //     .WithMessage("The person it does not exist.");

        //Validations1();
        //Validations2();
    }

    //private async Task<bool> Exists(UpdatePersonRequest request)
    //{
    //    var person = await _repository.GetById(request.PersonId);
    //    return person != null;
    //}

    //private async Task<bool> IsUnique(UpdatePersonRequest request)
    //{
    //    var people = await _repository.GetByNameAndStudyId(request.StudyId, request.ShortName);
    //    return !people.Any() || !people.Any(x => x.PersonId != request.PersonId);
    //}

    //private void Validations1()
    //{
    //    When(x => x.IsBeginDateSeg1 && x.IsEndDateSeg1, () =>
    //    {
    //        RuleFor(x => x.EndDateSeg1)
    //            .GreaterThanOrEqualTo(x => x.BeginDateSeg1)
    //            .WithMessage("The end date for Seg1 must be after the beggining date.");
    //    });

    //    When(x => x.IsFivePercentSeg1 && x.IsG5, () =>
    //    {
    //        RuleFor(x => x.G5iADateSeg1)
    //            .NotNull()
    //            .WithMessage("The  date is required.");
    //    });

    //    When(x => x.IsFivePercentSeg1 && x.IsG5iBSeg1, () =>
    //    {
    //        RuleFor(x => x.G5iBDateSeg1)
    //            .NotNull()
    //            .WithMessage("The  date is required.");
    //    });

    //    When(x => x.IsFivePercentSeg1 && x.IsTierDownSeg1, () =>
    //    {
    //        RuleFor(x => x.TierDownDateSeg1)
    //            .NotNull()
    //            .WithMessage("The Tier down date is required.");
    //    });

    //    When(x => x.LinkIdSeg1 != null && x.LinkIdSeg1 != 0, () =>
    //    {
    //        RuleFor(x => x.LinkIdSeg1)
    //            .NotEqual(x => x.PersonId)
    //            .WithMessage("Invalid link person.");
    //    });
    //}

    //private void Validations2()
    //{
    //    When(x => x.IsBeginDateSeg2 && x.IsEndDateSeg2, () =>
    //    {
    //        RuleFor(x => x.EndDateSeg2)
    //            .GreaterThanOrEqualTo(x => x.BeginDateSeg2)
    //            .WithMessage("The end date for Seg2 must be after the beggining date.");
    //    });

    //    When(x => x.IsFivePercentSeg2 && x.IsG5iASeg2, () =>
    //    {
    //        RuleFor(x => x.G5iADateSeg2)
    //            .NotNull()
    //            .WithMessage("The  date is required.");
    //    });

    //    When(x => x.IsFivePercentSeg2 && x.IsG5iBSeg2, () =>
    //    {
    //        RuleFor(x => x.G5iBDateSeg2)
    //            .NotNull()
    //            .WithMessage("The  date is required.");
    //    });

    //    When(x => x.IsFivePercentSeg2 && x.IsTierDownSeg2, () =>
    //    {
    //        RuleFor(x => x.TierDownDateSeg2)
    //            .NotNull()
    //            .WithMessage("The Tier down date is required.");
    //    });

    //    When(x => x.LinkIdSeg2 != null && x.LinkIdSeg2 != 0, () =>
    //    {
    //        RuleFor(x => x.LinkIdSeg2)
    //            .NotEqual(x => x.PersonId)
    //            .WithMessage("Invalid link person.");
    //    });

    //}
}

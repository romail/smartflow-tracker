namespace SmartFlow.Tracker.Application.Tasks.Validators
{
    using FluentValidation;
    using SmartFlow.Tracker.Application.Tasks.Models;

    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200);
        }
    }
}


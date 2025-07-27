namespace SmartFlow.Tracker.Application.Tasks.Validators
{
    using FluentValidation;
    using SmartFlow.Tracker.Application.Tasks.Models;

    public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
    {
        public UpdateTaskRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200);

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(status => status == "Todo" || status == "InProgress" || status == "Done")
                .WithMessage("Status must be one of: Todo, InProgress, Done");
        }
    }
}
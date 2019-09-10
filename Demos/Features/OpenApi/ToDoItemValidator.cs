namespace Demos.Features.OpenApi
{
    using FluentValidation;

    public class ToDoItemValidator : AbstractValidator<TodoItem>
    {
        public ToDoItemValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
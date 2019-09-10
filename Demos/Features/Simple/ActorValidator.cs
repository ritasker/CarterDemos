namespace Demos.Features.Simple
{
    using FluentValidation;

    public class ActorValidator : AbstractValidator<Actor>
    {
        public ActorValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Age).GreaterThan(0);
        }
    }
}
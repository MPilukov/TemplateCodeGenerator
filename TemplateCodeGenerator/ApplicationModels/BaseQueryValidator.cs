using FluentValidation;

namespace BaseNamespace
{
    /// <summary>
    /// Валидатор для <see cref="BaseQueryModel"/>
    /// </summary>
    public class BaseQueryValidator : AbstractValidator<BaseQueryModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseQueryValidator"/> class.
        /// </summary>
        public BaseQueryValidator()
        {
            RuleFor(c => c.ObjectId).NotEmpty().WithMessage("Идентификато объекта должен быть указан");
        }
    }
}
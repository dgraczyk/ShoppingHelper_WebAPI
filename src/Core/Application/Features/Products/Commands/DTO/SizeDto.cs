using Domain.Enums;
using FluentValidation;

namespace Application.Features.Products.Commands.DTO
{
    public class SizeDto
    {
        public decimal Value { get; set; }
        public string Unit { get; set; }

        public class Validator : AbstractValidator<SizeDto>
        {
            public Validator()
            {
                RuleFor(x => x.Value)
                      .GreaterThan(0)
                      .ScalePrecision(2, 5);

                RuleFor(x => x.Unit)
                    .IsEnumName(typeof(SizeUnits));
            }
        }
    }
}

﻿using Domain.Enums;
using FluentValidation;

namespace Application.Features.Products.Commands.DTO
{
    public class BasePriceDto
    {
        public decimal Price { get; set; }
        public decimal? PricePerSizeUnit { get; set; }
        public string PriceSizeUnitType { get; set; }

        public class Validator : AbstractValidator<BasePriceDto>
        {
            public Validator()
            {
                RuleFor(x => x.Price)
                       .GreaterThan(0)
                       .ScalePrecision(2, 8);

                When(x => x.PricePerSizeUnit.HasValue, () =>
                {
                    RuleFor(x => x.PricePerSizeUnit)
                        .GreaterThan(0)
                        .ScalePrecision(2, 8);

                    RuleFor(x => x.PriceSizeUnitType)
                        .IsEnumName(typeof(SizeUnits));
                });
            }
        }
    }
}

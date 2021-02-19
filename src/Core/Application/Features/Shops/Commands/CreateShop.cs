using Application.Contracts.Persistence;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Shops.Commands
{
    public class CreateShop
    {
        public class CreateShopCommand : IRequest<int>
        {
            public string Name { get; set; }
        }

        public class Validator : AbstractValidator<CreateShopCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(2)
                    .MaximumLength(50);
            }
        }

        public class Handler : IRequestHandler<CreateShopCommand, int>
        {
            private readonly IShopRepository shopRepository;
            private readonly Validator validator;

            public Handler(IShopRepository shopRepository)
            {
                this.shopRepository = shopRepository;
                this.validator = new Validator();
            }

            public async Task<int> Handle(CreateShopCommand request, CancellationToken cancellationToken)
            {
                await this.validator.ValidateAndThrowAsync(request, cancellationToken);

                var shop = new Shop { Name = request.Name };

                await this.shopRepository.AddAsync(shop);

                return shop.Id;
            }
        }
    }
}

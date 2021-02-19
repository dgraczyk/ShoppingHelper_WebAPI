using Application.Contracts.Persistence;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Categories.Commands
{
    public class CreateCategory
    {
        public class CreateCategoryCommand : IRequest<int>
        {
            public string Name { get; set; }
        }

        public class Validator : AbstractValidator<CreateCategoryCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .NotEmpty()
                    .MaximumLength(200);
            }
        }

        public class Handler : IRequestHandler<CreateCategoryCommand, int>
        {
            private readonly ICategoryRepository categoryRepository;
            private readonly Validator validator;

            public Handler(ICategoryRepository categoryRepository)
            {
                this.categoryRepository = categoryRepository;
                this.validator = new Validator();
            }

            public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                await this.validator.ValidateAndThrowAsync(request, cancellationToken);
                
                var category = new Category { Name = request.Name };
                
                await this.categoryRepository.AddAsync(category);

                return category.Id;
            }
        }

    }
}

using System.Collections.Generic;

namespace Application.Features.Products.Queries.DTO
{
    public class ProductOffertDto
    {
        public string Shop { get; set; }
        public List<PriceDto> Prices { get; set; }
    }
}

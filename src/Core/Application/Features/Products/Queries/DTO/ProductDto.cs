using System.Collections.Generic;

namespace Application.Features.Products.Queries.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string Category { get; set; }
        public string Size { get; set; }
        public List<ProductOffertDto> Offerts { get; set; }
    }
}

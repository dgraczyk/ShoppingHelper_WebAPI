namespace Application.Features.Products.Queries.DTO
{
    public class PriceDto
    {
        public decimal PriceValue { get; set; }
        public string PricePerSizeUnit { get; set; }
        public string Created { get; set; }
        public bool IsPromotionPrice { get; set; }
        public string PromotionConstraints { get; set; }
    }
}

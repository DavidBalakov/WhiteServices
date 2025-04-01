namespace Diploma.DTO
{
    public class OrdersSearch
    {
        public string? ClientName { get; set; }
        public DateOnly? Date { get; set; }
        public string? ProductModel { get; set; }
        public string? ProductBrand { get; set; }
    }
}
namespace Diploma.DTO
{
    public class OrdersDTO
    {
        public string Id { get; set; }
        public string ClientName { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public string RepairType { get; set; }
        public string OrderStatus { get; set; }
        public string ProductBrand { get; set; }
        public string ProductModel { get; set; }
        public string AdditionalNotes { get; set; }
    }
}
namespace LaptopRentalManagement.Models.Dashboard
{
    public class RecentOrder
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string LaptopModel { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }
}

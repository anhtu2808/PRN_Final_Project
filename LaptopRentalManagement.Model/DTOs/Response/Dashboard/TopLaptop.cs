namespace LaptopRentalManagement.Model.DTOs.Response.Dashboard
{
    public class TopLaptop
    {
        public string Model { get; set; }
        public string Brand { get; set; }
        public int RentCount { get; set; }
        public decimal Revenue { get; set; }
    }
}

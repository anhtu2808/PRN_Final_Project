namespace LaptopRentalManagement.BLL.DTOs.Response
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string IconClass { get; set; } = "fas fa-tag";
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LaptopCount { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.BLL.DTOs.Request
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên danh mục không được quá 100 ký tự")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "Mô tả không được quá 500 ký tự")]
        public string? Description { get; set; }
    }
} 
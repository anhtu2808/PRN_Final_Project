using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LaptopRentalManagement.Pages.Laptops
{
    public class DetailsModel : PageModel
    {
        public int LaptopId { get; set; }
        
        public void OnGet(int id)
        {
            LaptopId = id;
            
            // In a real application, you would load laptop details from database here
            // var laptop = _laptopService.GetLaptopById(id);
            // if (laptop == null)
            // {
            //     return NotFound();
            // }
        }
    }
} 
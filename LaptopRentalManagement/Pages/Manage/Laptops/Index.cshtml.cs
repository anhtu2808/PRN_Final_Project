using LaptopRentalManagement.BLL.DTOs.Request;
using LaptopRentalManagement.BLL.DTOs.Response;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaptopRentalManagement.Pages.Manage.Laptops
{
    public class IndexModel : PageModel
    {
        private readonly ILaptopService _laptopService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;

        public IList<LaptopResponse> Laptops { get; set; } = new List<LaptopResponse>();

        [BindProperty(SupportsGet = true)] public LaptopFilter Filter { get; set; } = new();

        [BindProperty] public EditLaptopRequest EditForm { get; set; }

        [BindProperty] public CreateLaptopRequest CreateForm { get; set; }

        public SelectList CategorySelect { get; set; }
        public SelectList BrandSelect { get; set; }
        public SelectList AccountSelect { get; set; }
        public SelectList StatusSelect { get; set; }

        public IndexModel(
            ILaptopService laptopService,
            ICategoryService categoryService,
            IBrandService brandService,
            IAccountService accountService
        )
        {
            _laptopService = laptopService;
            _categoryService = categoryService;
            _brandService = brandService;
            _accountService = accountService;
        }

        public async Task OnGetAsync()
        {
            var cats = await _categoryService.GetAllCategoriesAsync();
            var brands = await _brandService.GetAllBrandsAsync();
            var users = await _accountService.GetAll();

            CategorySelect = new SelectList(cats, "CategoryId", "Name");
            BrandSelect = new SelectList(brands, "BrandId", "Name");
            AccountSelect = new SelectList(users, "AccountId", "Name");
            StatusSelect = new SelectList(new[]
            {
                new { Value = "Available", Text = "Available" },
                new { Value = "PendingApproval", Text = "Pending Approval" },
                new { Value = "Rented", Text = "Rented" }
            }, "Value", "Text");

            Laptops = await _laptopService.GetAllAsync(Filter);
        }

        public async Task<JsonResult> OnGetGetLaptopAsync(int id)
        {
            var laptop = await _laptopService.GetByIdAsync(id);
            if (laptop == null)
            {
                return new JsonResult(null) { StatusCode = 404 };
            }

            return new JsonResult(new
            {
                laptopId = laptop.LaptopId,
                name = laptop.Name,
                brandId = laptop.Brand.BrandId,
                accountId = laptop.Owner.AccountId,
                imageUrl = laptop.ImageURL,
                categoryIds = laptop.Categories.Select(c => c.CategoryId).ToList(),
                pricePerDay = laptop.PricePerDay,
                cpu = laptop.Cpu,
                ram = laptop.Ram,
                storage = laptop.Storage
            });
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            var request = new EditLaptopRequest
            {
                LaptopId = EditForm.LaptopId,
                Name = EditForm.Name,
                BrandId = EditForm.BrandId,
                // AccountId = EditForm.AccountId,
                PricePerDay = EditForm.PricePerDay,
                Cpu = EditForm.Cpu,
                Ram = EditForm.Ram,
                Storage = EditForm.Storage,
                CategoryIds = EditForm.CategoryIds,
                ImageFile = EditForm.ImageFile 
            };

            await _laptopService.UpdateAsync(request);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            var request = new CreateLaptopRequest
            {
                Name = CreateForm.Name,
                Description = CreateForm.Description,
                ImageFile = CreateForm.ImageFile,
                BrandId = CreateForm.BrandId,
                AccountId = CreateForm.AccountId,
                PricePerDay = CreateForm.PricePerDay,
                Cpu = CreateForm.Cpu,
                Ram = CreateForm.Ram,
                Storage = CreateForm.Storage,
                CategoryIds = CreateForm.CategoryIds,
            };

            await _laptopService.CreateAsync(request);

            return RedirectToPage();
        }
    }
}
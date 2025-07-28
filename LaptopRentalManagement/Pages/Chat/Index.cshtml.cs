using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using LaptopRentalManagement.Model.DTOs.Response.Chat;
using System.Security.Claims;

namespace LaptopRentalManagement.Pages.Chat
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IChatService _chatService;

        public IndexModel(IChatService chatService)
        {
            _chatService = chatService;
        }

        [BindProperty]
        public CreateChatRoomRequest CreateChatRoomForm { get; set; } = new();

        [BindProperty]
        public SendMessageRequest SendMessageForm { get; set; } = new();

        [BindProperty]
        public AssignStaffRequest AssignStaffForm { get; set; } = new();

        public List<ChatRoomResponse> ChatRooms { get; set; } = new();
        public ChatRoomDetailResponse? CurrentChatRoom { get; set; }
        public ChatSummaryResponse? ChatSummary { get; set; }
        public string? CurrentUserRole { get; set; }
        public int? CurrentUserId { get; set; }
        public int? CurrentChatRoomId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? chatRoomId = null)
        {
            CurrentUserId = GetCurrentUserId();
            CurrentUserRole = GetCurrentUserRole();
            CurrentChatRoomId = chatRoomId;

            if (CurrentUserId == null || CurrentUserRole == null)
                return Unauthorized();

            try
            {
                // Load chat rooms
                await LoadChatRoomsAsync();

                // Load current chat room if specified
                if (chatRoomId.HasValue)
                {
                    var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId.Value, CurrentUserId.Value, CurrentUserRole);
                    if (canAccess)
                    {
                        CurrentChatRoom = await _chatService.GetChatRoomWithMessagesAsync(chatRoomId.Value, CurrentUserId.Value, 1);
                    }
                }

                // Load chat summary for admin/staff
                if (CurrentUserRole == "Admin" || CurrentUserRole == "Staff")
                {
                    var staffId = CurrentUserRole == "Staff" ? CurrentUserId.Value : (int?)null;
                    ChatSummary = await _chatService.GetChatSummaryAsync(staffId);
                }

                return Page();
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to load chat data";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCreateChatRoomAsync()
        {
            CurrentUserId = GetCurrentUserId();
            CurrentUserRole = GetCurrentUserRole();

            

            try
            {
                // Ensure customer can only create chat rooms for themselves
                CreateChatRoomForm.CustomerId = CurrentUserId.Value;

                var chatRoom = await _chatService.CreateChatRoomAsync(CreateChatRoomForm);
                TempData["SuccessMessage"] = "Chat room created successfully";
                return RedirectToPage("/Chat/Index", new { chatRoomId = chatRoom.ChatRoomId });
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to create chat room";
                await LoadChatRoomsAsync();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAssignStaffAsync()
        {
            CurrentUserId = GetCurrentUserId();
            CurrentUserRole = GetCurrentUserRole();

            if (CurrentUserId == null || CurrentUserRole != "Admin")
                return Unauthorized();
            
            try
            {
                var success = await _chatService.AssignStaffAsync(AssignStaffForm);
                if (success)
                {
                    TempData["SuccessMessage"] = "Staff assigned successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to assign staff";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to assign staff";
            }

            return RedirectToPage("/Chat/Index", new { chatRoomId = AssignStaffForm.ChatRoomId });
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int chatRoomId, string status)
        {
            CurrentUserId = GetCurrentUserId();
            CurrentUserRole = GetCurrentUserRole();

            if (CurrentUserId == null || (CurrentUserRole != "Admin" && CurrentUserRole != "Staff"))
                return Unauthorized();

            try
            {
                var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId, CurrentUserId.Value, CurrentUserRole);
                if (!canAccess)
                    return Forbid();

                var success = await _chatService.UpdateChatRoomStatusAsync(chatRoomId, status);
                if (success)
                {
                    TempData["SuccessMessage"] = "Status updated successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update status";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to update status";
            }

            return RedirectToPage("/Chat/Index", new { chatRoomId });
        }

        public async Task<IActionResult> OnPostMarkAsReadAsync(int chatRoomId)
        {
            CurrentUserId = GetCurrentUserId();
            CurrentUserRole = GetCurrentUserRole();

            if (CurrentUserId == null || CurrentUserRole == null)
                return Unauthorized();

            try
            {
                var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId, CurrentUserId.Value, CurrentUserRole);
                if (!canAccess)
                    return Forbid();

                await _chatService.MarkMessagesAsReadAsync(chatRoomId, CurrentUserId.Value);
                TempData["SuccessMessage"] = "Messages marked as read";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to mark messages as read";
            }

            return RedirectToPage("/Chat/Index", new { chatRoomId });
        }

        private async Task LoadChatRoomsAsync()
        {
            if (CurrentUserId == null || CurrentUserRole == null)
                return;

            try
            {
                IEnumerable<ChatRoomResponse> chatRooms;

                if (CurrentUserRole == "Customer")
                {
                    chatRooms = await _chatService.GetCustomerChatRoomsAsync(CurrentUserId.Value);
                }
                else if (CurrentUserRole == "Admin")
                {
                    chatRooms = await _chatService.GetUnassignedChatRoomsAsync();
                }
                else if (CurrentUserRole == "Staff")
                {
                    chatRooms = await _chatService.GetStaffChatRoomsAsync(CurrentUserId.Value);
                }
                else
                {
                    chatRooms = new List<ChatRoomResponse>();
                }

                ChatRooms = chatRooms.ToList();
            }
            catch (Exception)
            {
                ChatRooms = new List<ChatRoomResponse>();
            }
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                             User.FindFirst("AccountId")?.Value;
            
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }

        private string? GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
} 
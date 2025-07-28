using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LaptopRentalManagement.BLL.Interfaces;
using LaptopRentalManagement.Model.DTOs.Request;
using System.Security.Claims;

namespace LaptopRentalManagement.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("rooms")]
    public async Task<IActionResult> GetChatRooms()
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null)
            return Unauthorized();

        try
        {
            IEnumerable<object> chatRooms;

            if (userRole == "Customer")
            {
                chatRooms = await _chatService.GetCustomerChatRoomsAsync(userId.Value);
            }
            else if (userRole == "Admin")
            {
                var allRooms = await _chatService.GetUnassignedChatRoomsAsync();
                chatRooms = allRooms;
            }
            else if (userRole == "Staff")
            {
                chatRooms = await _chatService.GetStaffChatRoomsAsync(userId.Value);
            }
            else
            {
                return Forbid();
            }

            return Ok(chatRooms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to retrieve chat rooms", error = ex.Message });
        }
    }

    [HttpGet("rooms/{chatRoomId}")]
    public async Task<IActionResult> GetChatRoom(int chatRoomId, [FromQuery] int page = 1)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null)
            return Unauthorized();

        try
        {
            var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId, userId.Value, userRole);
            if (!canAccess)
                return Forbid();

            var chatRoom = await _chatService.GetChatRoomWithMessagesAsync(chatRoomId, userId.Value, page);
            if (chatRoom == null)
                return NotFound();

            return Ok(chatRoom);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to retrieve chat room", error = ex.Message });
        }
    }

    [HttpPost("rooms")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomRequest request)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        // Ensure customer can only create chat rooms for themselves
        if (request.CustomerId != userId.Value)
            return Forbid();

        try
        {
            var chatRoom = await _chatService.CreateChatRoomAsync(request);
            return CreatedAtAction(nameof(GetChatRoom), new { chatRoomId = chatRoom.ChatRoomId }, chatRoom);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to create chat room", error = ex.Message });
        }
    }

    [HttpPost("rooms/{chatRoomId}/assign")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignStaff(int chatRoomId, [FromBody] AssignStaffRequest request)
    {
        if (request.ChatRoomId != chatRoomId)
            return BadRequest("ChatRoomId mismatch");

        try
        {
            var success = await _chatService.AssignStaffAsync(request);
            if (!success)
                return BadRequest("Failed to assign staff");

            return Ok(new { message = "Staff assigned successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to assign staff", error = ex.Message });
        }
    }

    [HttpPut("rooms/{chatRoomId}/status")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> UpdateChatRoomStatus(int chatRoomId, [FromBody] UpdateChatRoomStatusRequest request)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null)
            return Unauthorized();

        try
        {
            var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId, userId.Value, userRole);
            if (!canAccess)
                return Forbid();

            var success = await _chatService.UpdateChatRoomStatusAsync(chatRoomId, request.Status);
            if (!success)
                return BadRequest("Failed to update status");

            return Ok(new { message = "Status updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to update status", error = ex.Message });
        }
    }

    [HttpPost("rooms/{chatRoomId}/messages/read")]
    public async Task<IActionResult> MarkMessagesAsRead(int chatRoomId)
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null)
            return Unauthorized();

        try
        {
            var canAccess = await _chatService.CanAccessChatRoomAsync(chatRoomId, userId.Value, userRole);
            if (!canAccess)
                return Forbid();

            var markedCount = await _chatService.MarkMessagesAsReadAsync(chatRoomId, userId.Value);
            return Ok(new { markedCount });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to mark messages as read", error = ex.Message });
        }
    }

    [HttpGet("summary")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetChatSummary()
    {
        var userId = GetCurrentUserId();
        var userRole = GetCurrentUserRole();

        if (userId == null || userRole == null)
            return Unauthorized();

        try
        {
            var staffId = userRole == "Staff" ? userId.Value : (int?)null;
            var summary = await _chatService.GetChatSummaryAsync(staffId);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to retrieve chat summary", error = ex.Message });
        }
    }

    [HttpGet("rooms/unassigned")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUnassignedChatRooms()
    {
        try
        {
            var chatRooms = await _chatService.GetUnassignedChatRoomsAsync();
            return Ok(chatRooms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Failed to retrieve unassigned chat rooms", error = ex.Message });
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

// Additional DTO for updating chat room status
public class UpdateChatRoomStatusRequest
{
    public string Status { get; set; } = null!;
} 
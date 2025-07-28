using LaptopRentalManagement.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace LaptopRentalManagement.BLL.DTOs.Request;

public class UpdateTicketRequest
{
    [Required]
    public int TicketId { get; set; }
    
    public string? Response { get; set; }
    
    [Required]
    public TicketStatus Status { get; set; }
} 
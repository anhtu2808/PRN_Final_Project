namespace LaptopRentalManagement.DAL.Entities;

public class Ticket
{
    public int         TicketId    { get; set; }
    public int         OrderId     { get; set; }

    public int         RenterId    { get; set; }
    public int?        OwnerId    { get; set; }    

    public string      Content     { get; set; }     // Nội dung khiếu nại
    public TicketStatus Status     { get; set; }     = TicketStatus.Open;
    public DateTime    CreatedAt   { get; set; }     = DateTime.UtcNow;
    public DateTime?   RespondedAt { get; set; }     // Thời gian staff phản hồi
    public string?     Response    { get; set; }     // Nội dung phản hồi

    // Navigation
    public virtual Order  Order    { get; set; }    
    public virtual Account Renter   { get; set; }   
    public virtual Account Owner  { get; set; }   
}
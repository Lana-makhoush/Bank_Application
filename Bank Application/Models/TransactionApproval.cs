using Bank_Application.Models;

public class TransactionApproval
{
    public int Id { get; set; }

    public int TransactionLogId { get; set; }
    public TransactionLog TransactionLog { get; set; } = null!;

    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public string RequiredRole { get; set; } = "Manager";

    public int? ApprovedByEmployeeId { get; set; }
    public Employee? ApprovedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ApprovedAt { get; set; }
}

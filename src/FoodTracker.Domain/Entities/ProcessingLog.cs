using System;

namespace FoodTracker.Domain.Entities;

public class ProcessingLog
{
    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }
    public Receipt? Receipt { get; set; }

    public string Step { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; }
}

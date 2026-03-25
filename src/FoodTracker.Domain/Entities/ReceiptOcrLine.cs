using System;

namespace FoodTracker.Domain.Entities;

public class ReceiptOcrLine
{
    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }
    public Receipt? Receipt { get; set; }

    public int LineNumber { get; set; }

    public string Text { get; set; } = null!;

    public decimal? Confidence { get; set; }

    public string? BoundingBoxJson { get; set; }

    public DateTime CreatedAt { get; set; }
}

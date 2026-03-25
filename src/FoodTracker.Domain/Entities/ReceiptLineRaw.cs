using System;

namespace FoodTracker.Domain.Entities;

public class ReceiptLineRaw
{
    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }
    public Receipt? Receipt { get; set; }

    public int? SourceOcrLineNumber { get; set; }

    public string RawText { get; set; } = null!;

    public decimal? DetectedPrice { get; set; }

    public decimal? DetectedQuantity { get; set; }

    public string? DetectedUnit { get; set; }

    public bool IsProductLine { get; set; }

    public string? IgnoredReason { get; set; }

    public decimal? ParseConfidence { get; set; }

    public DateTime CreatedAt { get; set; }
}

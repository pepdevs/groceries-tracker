using System;
using System.Collections.Generic;
using FoodTracker.Domain.Enums;

namespace FoodTracker.Domain.Entities;

public class Receipt
{
    public Guid Id { get; set; }

    public Guid? StoreId { get; set; }
    public Store? Store { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public TimeSpan? PurchaseTime { get; set; }

    public string? ReceiptNumber { get; set; }

    public decimal? TotalAmount { get; set; }

    public string Currency { get; set; } = "EUR";

    public string? OriginalFileName { get; set; }

    public string? StoredFilePath { get; set; }

    public string? FileHash { get; set; }

    public string? RawOcrText { get; set; }

    public OcrStatus OcrStatus { get; set; } = OcrStatus.Pending;

    public ParsingStatus ParsingStatus { get; set; } = ParsingStatus.Pending;

    public bool NeedsReview { get; set; } = true;

    public string? SourceType { get; set; }

    public string? DuplicateCheckStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<ReceiptOcrLine>? OcrLines { get; set; }
    public ICollection<ReceiptLineRaw>? RawLines { get; set; }
    public ICollection<PurchaseItem>? PurchaseItems { get; set; }
    public ICollection<ProcessingLog>? ProcessingLogs { get; set; }
}

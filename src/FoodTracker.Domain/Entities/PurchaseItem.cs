using System;
using FoodTracker.Domain.Enums;

namespace FoodTracker.Domain.Entities;

public class PurchaseItem
{
    public Guid Id { get; set; }

    public Guid ReceiptId { get; set; }
    public Receipt? Receipt { get; set; }

    public Guid? ReceiptLineRawId { get; set; }
    public ReceiptLineRaw? ReceiptLineRaw { get; set; }

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public Guid? StoreId { get; set; }
    public Store? Store { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string RawName { get; set; } = null!;

    public decimal? Quantity { get; set; }

    public string? Unit { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public decimal? DiscountAmount { get; set; }

    public bool WasAutoMatched { get; set; }

    public ReviewStatus ReviewStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

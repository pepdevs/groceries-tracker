using System;
using System.Collections.Generic;

namespace FoodTracker.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }

    public string NormalizedName { get; set; } = null!;

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public string? Subtype { get; set; }

    public string? Brand { get; set; }

    public decimal? SizeValue { get; set; }

    public string? SizeUnit { get; set; }

    public string? ComparableGroup { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<ProductAlias>? Aliases { get; set; }
    public ICollection<PurchaseItem>? PurchaseItems { get; set; }
}

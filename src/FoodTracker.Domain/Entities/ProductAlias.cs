using System;
using FoodTracker.Domain.Enums;

namespace FoodTracker.Domain.Entities;

public class ProductAlias
{
    public Guid Id { get; set; }

    public Guid? StoreId { get; set; }
    public Store? Store { get; set; }

    public string RawText { get; set; } = null!;

    public string NormalizedRawText { get; set; } = null!;

    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public MatchMethod MatchMethod { get; set; }

    public decimal? Confidence { get; set; }

    public bool CreatedFromManualReview { get; set; }

    public DateTime? LastUsedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}

using System;
using System.Collections.Generic;

namespace FoodTracker.Domain.Entities;

public class Store
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation
    public ICollection<Receipt>? Receipts { get; set; }
    public ICollection<ProductAlias>? Aliases { get; set; }
}

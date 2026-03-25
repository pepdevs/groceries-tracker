using System;
using System.Collections.Generic;

namespace FoodTracker.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public ICollection<Product>? Products { get; set; }
}

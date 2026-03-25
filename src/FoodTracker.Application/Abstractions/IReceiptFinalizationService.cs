using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodTracker.Domain.Entities;

namespace FoodTracker.Application.Abstractions;

public record ConfirmLineDto(Guid? ReceiptLineRawId, Guid? ProductId, bool Ignored, decimal? Quantity, decimal? UnitPrice, string? Unit);

public record ConfirmReceiptDto(Guid ReceiptId, Guid? StoreId, DateTime? PurchaseDate, IReadOnlyList<ConfirmLineDto> Lines);

public interface IReceiptFinalizationService
{
    Task FinalizeReceiptAsync(ConfirmReceiptDto confirmation);
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodTracker.Application.Abstractions;
using FoodTracker.Domain.Entities;

namespace FoodTracker.Application.Abstractions;

public record ParsedLineDto(string RawText, decimal? DetectedPrice, decimal? DetectedQuantity, string? DetectedUnit, bool IsProductLine, decimal? Confidence = null);

public record ReceiptParseResultDto(Guid? StoreId, DateTime? PurchaseDate, IReadOnlyList<ParsedLineDto> ParsedLines);

public interface IReceiptParser
{
    Task<ReceiptParseResultDto> ParseAsync(Guid receiptId, string rawOcrText, IReadOnlyList<OcrLineDto> ocrLines);
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodTracker.Domain.Entities;

namespace FoodTracker.Application.Abstractions;

public record OcrLineDto(int LineNumber, string Text, decimal? Confidence = null, string? BoundingBoxJson = null);

public record OcrResultDto(string FullText, IReadOnlyList<OcrLineDto> Lines);

public interface IOcrProvider
{
    Task<OcrResultDto> RunOcrAsync(Guid receiptId, byte[] imageContent);
}

using System;
using System.Threading.Tasks;
using FoodTracker.Application.Abstractions;
using FoodTracker.Domain.Entities;

namespace FoodTracker.Application.Abstractions;

public record ReceiptDraftDto(Receipt Receipt, IReadOnlyList<ReceiptLineRaw> RawLines, string? RawOcrText, IReadOnlyList<ProductMatchCandidateDto> CandidateMatches);

public interface IReceiptDraftService
{
    Task<ReceiptDraftDto> GetDraftAsync(Guid receiptId);
}

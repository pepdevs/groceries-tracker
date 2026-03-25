using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodTracker.Application.Abstractions;
using FoodTracker.Domain.Entities;
using FoodTracker.Domain.Enums;

namespace FoodTracker.Application.Abstractions;

public record ProductMatchCandidateDto(Guid? ProductId, MatchMethod Method, decimal? Confidence, string SuggestedName);

public interface IProductMatchingService
{
    Task<IReadOnlyList<ProductMatchCandidateDto>> FindCandidatesAsync(Guid receiptId, string rawLineText);
}

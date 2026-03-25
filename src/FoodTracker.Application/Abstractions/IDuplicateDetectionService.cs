using System;
using System.Threading.Tasks;
using FoodTracker.Domain.Entities;

namespace FoodTracker.Application.Abstractions;

public record DuplicateCheckResult(bool IsHardDuplicate, bool IsSoftDuplicate, string? Reason = null);

public interface IDuplicateDetectionService
{
    Task<DuplicateCheckResult> CheckDuplicatesAsync(Receipt receipt);
}

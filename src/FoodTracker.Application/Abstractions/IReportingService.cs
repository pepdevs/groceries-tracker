using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodTracker.Application.Abstractions;

public record SpendByStoreDto(Guid StoreId, string StoreName, decimal TotalAmount);
public record SpendByCategoryDto(Guid CategoryId, string CategoryName, decimal TotalAmount);

public interface IReportingService
{
    Task<IReadOnlyList<SpendByStoreDto>> GetSpendByStoreAsync(DateTime from, DateTime to);
    Task<IReadOnlyList<SpendByCategoryDto>> GetSpendByCategoryAsync(DateTime from, DateTime to);
}

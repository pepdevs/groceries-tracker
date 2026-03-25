using System;
using System.Threading.Tasks;
using FoodTracker.Application.Abstractions;

namespace FoodTracker.Application.Abstractions;

public interface IStoreParser
{
    string StoreCode { get; }
    bool DetectStore(string rawOcrText);
    Task<ReceiptParseResultDto> ParseAsync(Guid receiptId, string rawOcrText, IReadOnlyList<OcrLineDto> ocrLines);
}

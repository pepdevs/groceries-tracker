using System;
using System.Threading.Tasks;

namespace FoodTracker.Application.Abstractions;

public interface IReceiptStorageService
{
    Task<string> StoreFileAsync(Guid receiptId, string originalFileName, byte[] content);
    Task<byte[]?> GetFileAsync(string storedFilePath);
    Task<string> ComputeHashAsync(byte[] content);
}

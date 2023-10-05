using Microsoft.AspNetCore.Http;

namespace BookStoreClassLibrary.Core.Util
{
    public static class FileConverter
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

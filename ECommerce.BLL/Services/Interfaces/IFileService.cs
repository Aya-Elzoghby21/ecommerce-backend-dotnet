using Microsoft.AspNetCore.Http;

namespace ECommerce.Api.Services.Interfaces
{
    public interface IFileService
    {
        string SaveProductImage(IFormFile image);
    }
}

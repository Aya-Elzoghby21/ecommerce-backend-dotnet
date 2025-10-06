using ECommerce.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Api.Services.implementation
{
    public class FileService : IFileService
    {
        private readonly string _rootPath;

        public FileService(string rootPath)
        {
            _rootPath = rootPath;
        }

        public string SaveProductImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            var folderPath = Path.Combine(_rootPath, "images/products");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return "/images/products/" + fileName;
        }
    }
}

using Microsoft.AspNetCore.Hosting;

namespace WebApplication1.Utility
{
    public class Helper
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Helper(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string handleUploadFile(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath + @"/images/product");

            using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }
    }
}

namespace SWP391_M2_BL5_G4_SP25.Service
{
    public class UploadImg
    {
        private readonly IWebHostEnvironment _env;

        public UploadImg(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var fullFolderPath = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }

            var filePath = Path.Combine(fullFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về path để lưu vào DB (đường dẫn tương đối cho hiển thị ảnh)
            return "/" + folder.Replace("\\", "/") + "/" + fileName;
        }
    }
}

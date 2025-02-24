using Microsoft.AspNetCore.Http;

namespace _11_ActorManagementApp.ProjectModels
{
    public class FileUpload
    {
        private readonly string _webRootPath;
        public FileUpload(string webRootPath)
        {
            this._webRootPath = webRootPath;
        }
        public async Task<string> ImageUploadAsync(IFormFile file, string folderName)
        {
            return await UploadAsync(file, folderName, new[] { ".jpg", ".jpeg", ".png" },
                "Invalid file type, Only jpg, jpeg and png files are allowed.");
        }

        public async Task<string> ImageOrPdfUploadAsync(IFormFile file, string folderName)
        {
            return await UploadAsync(file, folderName, new[] { ".jpg", ".jpeg", ".png", ".pdf" },
                "Invalid file type, Only jpg, jpeg, png and pdf files are allowed.");
        }

        public async Task<string> PdfUploadAsync(IFormFile file, string folderName)
        {
            return await UploadAsync(file, folderName, new[] { ".pdf" },
                "Invalid file type, Only pdf files are allowed.");
        }

        private async Task<string> UploadAsync(IFormFile file, string folderName, string[] allowedExtensions, string errorMessage)
        {
            if (file == null || file.Length == 0)
                throw new InvalidOperationException("Invalid file.");

            //Generate a unique file name
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName).ToLower();

            //Validate file extension
            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException(errorMessage);

            //Append timestamp to ensure unique file name
            fileName = $"{fileName}_{DateTime.Now.ToString("yymmssfff")}{extension}";

            //Combine path
            string directoryPath = Path.Combine(_webRootPath, folderName);
            string filePath = Path.Combine(directoryPath, fileName);

            //Ensure the directory exists
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            //Save file in wwwroot/(foldername)
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName; //Return file name to save in database
        }

        public async Task<List<string>> MultipleImageUploadAsync(List<IFormFile> files, string folderName)
        {
            return await MultipleUploadAsync(files, folderName, new[] { ".jpg", ".jpeg", ".png" },
                "Invalid file type, Only jpg, jpeg and png files are allowed.");
        }

        public async Task<List<string>> MultipleImageOrPdfUploadAsync(List<IFormFile> files, string folderName)
        {
            return await MultipleUploadAsync(files, folderName, new[] { ".jpg", ".jpeg", ".png", ".pdf" },
                "Invalid file type, Only jpg, jpeg, png and pdf files are allowed.");
        }

        public async Task<List<string>> MultiplePdfUploadAsync(List<IFormFile> files, string folderName)
        {
            return await MultipleUploadAsync(files, folderName, new[] { ".pdf" },
                "Invalid file type, Only pdf files are allowed.");
        }

        private async Task<List<string>> MultipleUploadAsync(List<IFormFile> files, string folderName, string[] allowedExtensions, string errorMessage)
        {
            if (files == null || files.Count == 0)
                throw new InvalidOperationException("No files selected for upload.");

            List<string> uploadedFileNames = new List<string>();
            string directoryPath = Path.Combine(_webRootPath, folderName);

            //Ensure the drectory exists
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (var file in files)
            {
                if (file.Length == 0)
                    continue; //Skip empty files

                //Generate a unique file name
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName).ToLower();

                //Validate the file extension
                if (!allowedExtensions.Contains(extension))
                    throw new InvalidOperationException(errorMessage);

                //Append timestamp to ensure unique file name
                fileName = $"{fileName}_{DateTime.Now.ToString("yymmssfff")}{extension}";

                //Combine path
                string filePath = Path.Combine(directoryPath, fileName);

                //Save file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                uploadedFileNames.Add(fileName);
            }
            return uploadedFileNames; //Return list of uploaed file names to be saved in the database
        }
    }
}

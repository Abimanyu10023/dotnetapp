using System.IO;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ResumeUploader.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public ResumeController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetValue<string>("DefaultEndpointsProtocol=https;AccountName=abistore;AccountKey=H0s/4SnM9JQhnTOO/YwY8liTMv5ZdbBzwhnQhHsyfHRbr7ADvpv0OQVX7OBQww1/oYPG1s6rm7kr+ASt7LfL1g==;EndpointSuffix=core.windows.net");
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = _configuration.GetValue<string>("abi03-containerr");
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Get a reference to the Blob container
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

                // Create a unique name for the resume file
                string fileName = Path.GetFileName(file.FileName);
                string uniqueFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(fileName);

                // Upload the resume file to the Blob container
                using (Stream stream = file.OpenReadStream())
                {
                    containerClient.UploadBlob(uniqueFileName, stream);
                }

                ViewBag.Message = "Resume uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "No file selected for upload.";
            }

            return View();
        }
    }
}

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResumeUploader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string resumeFilePath = "https://abistore.blob.core.windows.net/abi03-containerr"; // Update with the actual file path of your resume

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Read the resume file as bytes
                    byte[] fileBytes = File.ReadAllBytes(resumeFilePath);

                    // Create a new MultipartFormDataContent
                    var multipartContent = new MultipartFormDataContent();

                    // Convert the file bytes to a StreamContent
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "resume",
                        FileName = Path.GetFileName(resumeFilePath)
                    };

                    // Add the file content to the MultipartFormDataContent
                    multipartContent.Add(fileContent);

                    // Send the HTTP POST request to the server
                    var response = await httpClient.PostAsync("https://your-upload-api-endpoint", multipartContent);

                    // Check if the upload was successful
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Resume uploaded successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Resume upload failed. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}

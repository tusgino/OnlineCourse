using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public UploadController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            var fileId = await UploadFile(file);
            var contentLink = await GetFileContentLink(fileId);

            // Do something with fileId and contentLink

            return Ok(new {ContentLink = contentLink });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private async Task<string> UploadFile(IFormFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);

            var credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "credentials.json");

            var scopes = new[] { DriveService.Scope.Drive };
            var credential = GoogleCredential.FromFile(credentialsPath).CreateScoped(scopes);
            var driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });

            var fileName = Path.GetFileNameWithoutExtension(file.FileName); // Lấy tên tệp tin ban đầu không bao gồm phần mở rộng
            var fileExtension = Path.GetExtension(file.FileName); // Lấy phần mở rộng của tệp tin ban đầu
            var randomFileName = $"{Path.GetRandomFileName()}{fileExtension}"; // Tạo tên ngẫu nhiên cho tệp tin

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = randomFileName, // Sử dụng tên ngẫu nhiên cho tệp tin
                Parents = new List<string> { "1P6PaxyrOnVOuhnpvp0Vj2JqAFZMPtDLR" }
            };

            var request = driveService.Files.Create(fileMetadata, stream, file.ContentType);
            request.Fields = "id";

            var uploadProgress = await request.UploadAsync();
            if (uploadProgress.Status == UploadStatus.Completed)
            {
                var uploadedFile = request.ResponseBody;
                Console.WriteLine($"Uploaded file {uploadedFile.Name} {uploadedFile.Id}");
                return uploadedFile.Id;
            }
            else
            {
                throw new Exception($"Failed to upload file: {fileName}");
            }
        }
    }

    private async Task<string> GetFileContentLink(string fileId)
    {
        var credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "credentials.json");

        var scopes = new[] { DriveService.Scope.Drive };
        var credential = GoogleCredential.FromFile(credentialsPath).CreateScoped(scopes);
        var driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential
        });
        var request = driveService.Files.Get(fileId);
        request.Fields = "webContentLink";
        var file = await request.ExecuteAsync();
        var contentLink = file.WebContentLink;

        return contentLink;
    }
}

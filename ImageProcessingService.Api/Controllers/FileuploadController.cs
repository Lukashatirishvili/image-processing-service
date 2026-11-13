using Microsoft.AspNetCore.Mvc;

namespace ImageProcessingService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileuploadController : ControllerBase
{
    private readonly UploadHandler _uploadHandler;

    public FileuploadController(UploadHandler uploadHandler)
    {
        _uploadHandler = uploadHandler;
    }
    
    [HttpPost("uploadfile")]
    public IActionResult UploadFile(IFormFile formFile)
    {
        var fileName = _uploadHandler.Upload(formFile);
        
        return Ok(fileName);
    }
}
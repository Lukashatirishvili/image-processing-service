namespace ImageProcessingService.Api;

public class UploadHandler
{
    public string Upload(IFormFile formFile)
    {
        // uploaded file extension
        var extension = Path.GetExtension(formFile.FileName);
        List<string> possibleExtensions = [".png", ".jpg", ".jpeg", ".heic", ".heif"];

        if (!possibleExtensions.Contains(extension))
        {
            return $"Extension is not valid";
        }

        // file size
        var sizeInMb = formFile.Length / (1024.0 * 1024.0);
        if (sizeInMb > 5)
        {
            return "size is more than 5mb";
        }
        
        
        // name changing
        var fileName = Guid.NewGuid().ToString() + extension;
        var path = "/Users/lukashatirishvili/newFolder";

        Directory.CreateDirectory(path);

        var fullPath = Path.Combine(path, fileName);
        
        

        FileStream stream = new FileStream(fullPath, FileMode.CreateNew);
        
        formFile.CopyTo(stream);
        stream.Dispose();
        stream.Close();

        return fileName;
    }
}
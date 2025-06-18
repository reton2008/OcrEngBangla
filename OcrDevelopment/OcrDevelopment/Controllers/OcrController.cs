using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Filters;
using Tesseract;

namespace OcrDevelopment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OcrController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public OcrController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("extract-text")]
    public async Task<IActionResult> ExtractText([FromForm] IFormFile image, [FromForm] string language = "eng+ben")
    {
        if (image == null || image.Length == 0)
            return BadRequest("No image uploaded.");

        var uploadDir = Path.Combine(_env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadDir);
        var originalFilePath = Path.Combine(uploadDir, Guid.NewGuid() + Path.GetExtension(image.FileName));

        await using (var stream = new FileStream(originalFilePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        var enhancedFilePath = Path.Combine(uploadDir, Guid.NewGuid() + ".png");

        try
        {
            using (var inputImage = await Image.LoadAsync<Rgba32>(originalFilePath))
            {
                inputImage.Mutate(x => x
                    .Grayscale()
                    .Contrast(1.5f)            // Higher contrast for better edge detection
                    .Brightness(1.2f)          // Slight brightness boost
                    .GaussianSharpen(1.0f)             // Sharpen more aggressively
                    .Resize(inputImage.Width * 2, inputImage.Height * 2) // Upscale to improve DPI for OCR
                );

                // ➔ Apply threshold (binarization) to convert to black & white for stronger OCR contrast
                inputImage.Mutate(c => c.BinaryThreshold(0.5f)); // 0.5f = 50% threshold, adjustable

                await inputImage.SaveAsync(enhancedFilePath, new PngEncoder());
            }

            var tessDataPath = Path.Combine(_env.ContentRootPath, "tessdata");

            using var engine = new TesseractEngine(tessDataPath, language, EngineMode.Default);
            using var img = Pix.LoadFromFile(enhancedFilePath);
            using var page = engine.Process(img);
            var extractedText = page.GetText();

            return Ok(new { text = extractedText });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"OCR failed: {ex.Message}");
        }
        finally
        {
            if (System.IO.File.Exists(originalFilePath))
                System.IO.File.Delete(originalFilePath);

            if (System.IO.File.Exists(enhancedFilePath))
                System.IO.File.Delete(enhancedFilePath);
        }
    }
}
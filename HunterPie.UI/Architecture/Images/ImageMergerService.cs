using SkiaSharp;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.UI.Architecture.Images;
public static class ImageMergerService
{

    /// <summary>
    /// Merges two images and saves it to the desired path
    /// </summary>
    /// <param name="outputPath">Path where the image should be saved</param>
    /// <param name="image">Image to be rendered under the mask</param>
    /// <param name="mask">The mask that will be rendered on top of image</param>
    /// <returns>Path to the saved file</returns>
    public static Task<string> MergeAsync(string outputPath, string image, string mask)
    {
        using var backgroundImage = SKImage.FromEncodedData(image);
        using var maskImage = SKImage.FromEncodedData(mask);
        using var bmp = SKBitmap.FromImage(backgroundImage);

        using (var graphics = new SKCanvas(bmp))
        {
            graphics.DrawImage(maskImage, new SKRect(0, 0, backgroundImage.Width, backgroundImage.Height));
            graphics.Flush();
        }

        using (var stream = File.OpenWrite(outputPath))
            bmp.Encode(stream, SKEncodedImageFormat.Png, 80);
        
        return Task.FromResult(outputPath);
    }
}

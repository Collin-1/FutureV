using System.ComponentModel.DataAnnotations;

namespace FutureV.Core.Entities;

public class CarImage
{
    public int Id { get; set; }

    [Required]
    public int CarId { get; set; }

    public Car? Car { get; set; }

    [Required, StringLength(200)]
    public string ViewAngle { get; set; } = string.Empty;

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    public byte[]? ImageData { get; set; }

    [StringLength(120)]
    public string? ContentType { get; set; }

    [StringLength(260)]
    public string? FileName { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    public string? GetImageSource()
    {
        if (ImageData is { Length: > 0 })
        {
            var contentType = string.IsNullOrWhiteSpace(ContentType) ? "image/png" : ContentType;
            return $"data:{contentType};base64,{Convert.ToBase64String(ImageData)}";
        }

        return string.IsNullOrWhiteSpace(ImageUrl) ? null : ImageUrl;
    }
}

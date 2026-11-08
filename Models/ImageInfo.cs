using System;

namespace ImageGalleryApp.Models;

public class ImageInfo
{
    public string FilePath { get; set; } = "";
    public string FileName { get; set; } = "";
    public DateTime CreationDate { get; set; }
    public long FileSize { get; set; }
    public string Format { get; set; } = "";
}
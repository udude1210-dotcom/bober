using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageGalleryApp.Models;

namespace ImageGalleryApp.Services;

public class ImageService
{
    private readonly string[] extensions =
    {
        ".jpg", ".jpeg", ".png", ".bmp"
    };

    public List<ImageInfo> LoadImages(string folders)
    {
        var result = new List<ImageInfo>();

        var paths = folders.Split(
            '@',
            StringSplitOptions.RemoveEmptyEntries |
            StringSplitOptions.TrimEntries
        );

        foreach (var path in paths)
        {
            if (!Directory.Exists(path))
                continue;

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                string extension = Path.GetExtension(file);

                if (!extensions.Contains(
                    extension,
                    StringComparer.OrdinalIgnoreCase))
                    continue;

                var fileInfo = new FileInfo(file);

                result.Add(new ImageInfo
                {
                    FilePath = file,
                    FileName = fileInfo.Name,
                    CreationDate = fileInfo.CreationTime,
                    FileSize = fileInfo.Length,
                    Format = extension.TrimStart('.').ToUpper()
                });
            }
        }

        return result.OrderBy(x => x.FileName).ToList();
    }
}
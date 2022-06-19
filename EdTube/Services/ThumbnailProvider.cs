using System.Collections.Concurrent;
using System.Drawing;
using FFMpegCore;

namespace EdTube.Services;

public interface IThumbnailProvider
{
    Task<string> GetVideoThumbnailAsync(int videoId, string videoPath);
}

public class ThumbnailProvider : IThumbnailProvider
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly Size _defaultThumbnailSize = new(320, 240);
    
    private readonly ConcurrentDictionary<int, string> _cache = new();

    private readonly string _thumbnailsPath;

    public ThumbnailProvider(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _thumbnailsPath = Path.Combine(hostEnvironment.WebRootPath, "Thumbnails");
        
        if (!Directory.Exists(_thumbnailsPath))
        {
            Directory.CreateDirectory(_thumbnailsPath);
        }
    }
    
    public async Task<string> GetVideoThumbnailAsync(int videoId, string videoPath)
    {
        if (_cache.TryGetValue(videoId, out var thumbnailPath))
        {
            return thumbnailPath;
        }

        var thumbnailDirectory = Path.Combine(_thumbnailsPath, videoId.ToString());
        if (!Directory.Exists(thumbnailDirectory))
        {
            Directory.CreateDirectory(thumbnailDirectory);
        }
        
        var videoName = Path.GetFileName(videoPath);
        var thumbnailName = Path.ChangeExtension(videoName, ".png");

        var absoluteVideoPath = Path.Combine(_hostEnvironment.WebRootPath, videoPath.TrimStart('\\'));
        var absolutePath = Path.Combine(thumbnailDirectory, thumbnailName);
        
        if (!File.Exists(absolutePath))
        {
            var mediaInfo = await FFProbe.AnalyseAsync(absoluteVideoPath);
            await FFMpeg.SnapshotAsync(absoluteVideoPath, absolutePath, _defaultThumbnailSize,
                TimeSpan.FromTicks(mediaInfo.Duration.Ticks / 2));
        }
        
        thumbnailPath = absolutePath.Replace(_hostEnvironment.WebRootPath, "");
        _cache.TryAdd(videoId, thumbnailPath);

        return thumbnailPath;
    }
}
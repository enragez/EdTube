using System.Collections.Concurrent;
using EdTube.Data.Entities;

namespace EdTube.Services;

public interface IVideoProvider
{
    Task<string> GetVideoPathAsync(Video video);
}

class VideoProvider : IVideoProvider
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly ConcurrentDictionary<int, string> _cache = new();

    private readonly string _videosPath;

    public VideoProvider(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _videosPath = Path.Combine(hostEnvironment.WebRootPath, "Videos");

        if (!Directory.Exists(_videosPath))
        {
            Directory.CreateDirectory(_videosPath);
        }
    }

    public async Task<string> GetVideoPathAsync(Video video)
    {
        if (_cache.TryGetValue(video.Id, out var path))
        {
            return path;
        }

        var videoDirectory = Path.Combine(_videosPath, video.Id.ToString());
        if (!Directory.Exists(videoDirectory))
        {
            Directory.CreateDirectory(videoDirectory);
        }
        
        var absolutePath = Path.Combine(videoDirectory, video.FileName);

        if (!File.Exists(absolutePath))
        {
            await File.WriteAllBytesAsync(absolutePath, video.Content);
        }
        
        path = absolutePath.Replace(_hostEnvironment.WebRootPath, "");
        _cache.TryAdd(video.Id, path);
        return path;
    }
}
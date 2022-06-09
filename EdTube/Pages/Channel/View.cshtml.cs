using System.Drawing;
using EdTube.Data;
using EdTube.Models;
using EdTube.Services;
using FFMpegCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Channel;

public class View : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IVideoProvider _videoProvider;
    private readonly IThumbnailProvider _thumbnailProvider;

    public View(ApplicationDbContext context, IVideoProvider videoProvider, IThumbnailProvider thumbnailProvider)
    {
        _context = context;
        _videoProvider = videoProvider;
        _thumbnailProvider = thumbnailProvider;
    }
    
    public VideoChannelModel ChannelModel { get; set; }
    
    public List<VideoModel> Videos { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        var channel = await _context.Channels.FirstAsync(c => c.Id == id.Value);
        ChannelModel = new VideoChannelModel
        {
            Id = channel.Id,
            Name = channel.Name,
            Poster = channel.Poster
        };

        var videos = _context.Videos
            .Where(v => v.Channel.Id == channel.Id);
        var videosList = new List<VideoModel>();
        foreach (var video in videos)
        {
            var videoPath = await _videoProvider.GetVideoPathAsync(video);
            videosList.Add(new VideoModel
            {
                Id = video.Id,
                Name = video.Name,
                ChannelModel = ChannelModel,
                FilePath = videoPath,
                ThumbnailFilePath = await _thumbnailProvider.GetVideoThumbnailAsync(video.Id, videoPath)
            });
        }

        Videos = videosList;
        
        return Page();
    }
}
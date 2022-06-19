using EdTube.Data;
using EdTube.Models;
using EdTube.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Video;

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
    
    [BindProperty]
    public VideoModel VideoModel { get; set; }
    
    public List<TestAnswerModel> Answers { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var video = await _context.Videos.Include(v => v.Channel).FirstAsync(v => v.Id == id);
        
        var channel = await _context.Channels.FirstAsync(c => c.Id == video.Channel.Id);
        var channelModel = new VideoChannelModel
        {
            Id = channel.Id,
            Name = channel.Name,
            Poster = channel.Poster
        };
        
        var videoPath = await _videoProvider.GetVideoPathAsync(video);
        VideoModel = new VideoModel
        {
            Id = video.Id,
            Name = video.Name,
            ChannelModel = channelModel,
            FilePath = videoPath,
            ThumbnailFilePath = await _thumbnailProvider.GetVideoThumbnailAsync(video.Id, videoPath),
            Questions = await _context.TestQuestions
                .Where(q => q.Video.Id == id)
                .Select(q => new TestQuestionModel
                                {
                                    Index = q.Index,
                                    Question = q.Question,
                                    FirstOption = q.FirstOption,
                                    SecondOption = q.SecondOption,
                                    ThirdOption = q.ThirdOption,
                                    FourthOption = q.FourthOption,
                                    RightOption = q.RightOption,
                                    VideoId = id
                                }).ToListAsync()
        };

        return Page();
    }
}
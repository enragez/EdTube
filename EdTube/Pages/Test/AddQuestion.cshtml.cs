using EdTube.Data;
using EdTube.Data.Entities;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Test;

public class AddQuestion : PageModel
{
    private readonly ApplicationDbContext _context;

    public AddQuestion(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public TestQuestionModel Model { get; set; }
    
    public void OnGet(int videoId, int index)
    {
        Model = new TestQuestionModel
        {
            Index = index,
            VideoId = videoId
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var video = await _context.Videos.FirstAsync(v => v.Id == Model.VideoId);
        var newQuestion = new TestQuestion
        {
            Index = Model.Index,
            Video = video,
            Question = Model.Question,
            FirstOption = Model.FirstOption,
            SecondOption = Model.SecondOption,
            ThirdOption = Model.ThirdOption,
            FourthOption = Model.FourthOption,
            RightOption = Model.RightOption
        };

        _context.TestQuestions.Add(newQuestion);

        await _context.SaveChangesAsync();

        return RedirectToPage("./AddQuestion", new { videoId = Model.VideoId, index = Model.Index + 1 });
    }
}
using EdTube.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Controllers;

public class QuizController : Controller
{
    private readonly ApplicationDbContext _context;

    public QuizController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswers(Dictionary<int, string> answers, int videoId)
    {
        var questions = await _context.TestQuestions.Where(q => q.Video.Id == videoId).ToListAsync();

        var correctCounter = 0;

        foreach (var question in questions)
        {
            if (answers.TryGetValue(question.Index, out var answer))
            {
                switch (answer)
                {
                    case "1":
                        if (question.RightOption == "1")
                        {
                            correctCounter++;
                        }

                        break;
                    case "2":
                        if (question.RightOption == "2")
                        {
                            correctCounter++;
                        }

                        break;
                    case "3":
                        if (question.RightOption == "3")
                        {
                            correctCounter++;
                        }

                        break;
                    case "4":
                        if (question.RightOption == "4")
                        {
                            correctCounter++;
                        }

                        break;
                }
            }
        }

        return Json(new
        {
            correctCounter,
            questionCount = questions.Count
        });
    }
}
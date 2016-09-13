using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using BasicAuthentication.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BasicAuthentication.Controllers
{
    [Authorize]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            return View(_db.Questions.Where(x => x.User.Id == currentUser.Id));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            question.User = currentUser;
            _db.Questions.Add(question);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Answer(int questionId)
        {
            ViewBag.QuestionId = questionId;
            return View();
        }
        [HttpPost]
        public IActionResult Answer(Answer answer)
        {
            _db.Answers.Add(answer);
            _db.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult AnswerDisplay(int currentQuestionId)
        {
            var currentQuestion = _db.Questions.Include(question => question.Answers).FirstOrDefault(questions => questions.Id == currentQuestionId);
            return View(currentQuestion);
        }

        public IActionResult AnswerUpvote(int currentAnswerId)
        {
            Answer ratedAnswer = _db.Answers.FirstOrDefault(answers => answers.Id == currentAnswerId);
            ratedAnswer.Rating++;
            _db.SaveChanges();
            return RedirectToAction("AnswerDisplay", new { currentQuestionId = ratedAnswer.QuestionId });
        }
    }
}

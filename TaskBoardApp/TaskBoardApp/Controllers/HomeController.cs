using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoardApp.Data;
using TaskBoardApp.Models;

namespace TaskBoardApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext data;

        public HomeController(ApplicationDbContext context)
        {
            data = context;
        }

        public IActionResult Index()
        {
            var taskBoards = data.Boards.Select(x => x.Name).Distinct().ToList();

            var tasksCount = new List<HomeBoardModel>();
            foreach (var taskBoard in taskBoards)
            {
                var tasksInBoard = data.Tasks.Where(x => x.Board.Name == taskBoard).Count();
                tasksCount.Add(new HomeBoardModel
                {
                    BoardName = taskBoard,
                    TasksCount = tasksInBoard
                });
            }
            var userTasksCount = -1;

            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                userTasksCount = data.Tasks.Where(x => x.OwnerId == currentUserId).Count();
            }

            var homeModel = new HomeViewModel
            {
                AllTasksCount = data.Tasks.Count(),
                BoardsWithTasksCount = tasksCount,
                UserTasksCount = userTasksCount
            };

            return View(homeModel);
        }
    }
}
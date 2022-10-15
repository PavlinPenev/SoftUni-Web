using Microsoft.AspNetCore.Mvc;
using TaskBoardApp.Data;
using TaskBoardApp.Models;
using TaskBoardApp.Models.Task;

namespace TaskBoardApp.Controllers
{
    public class BoardsController : Controller
    {
        private readonly ApplicationDbContext data;

        public BoardsController(ApplicationDbContext context)
        {
            this.data = context;
        }

        public IActionResult All()
        {
            var boards = this.data.Boards.
                Select(x => new BoardViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Tasks = x.Tasks.Select(y => new TaskViewModel
                    {
                        Id = y.Id,
                        Title = y.Title,
                        Description = y.Description,
                        Owner = y.Owner.UserName
                    })
                }).ToList();

            return View(boards);
        }
    }
}

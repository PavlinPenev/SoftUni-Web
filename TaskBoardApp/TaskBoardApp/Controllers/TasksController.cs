using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoardApp.Data;
using TaskBoardApp.Models.Task;
using Task = TaskBoardApp.Data.Entities.Task;

namespace TaskBoardApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext data;


        public TasksController(ApplicationDbContext context)
        {
            this.data = context;
        }

        public IActionResult Create()
        {
            TaskFormModel taskModel = new TaskFormModel
            {
                Boards = GetBoards()
            };
            return View(taskModel);
        }

        [HttpPost]
        public IActionResult Create(TaskFormModel taskFormModel)
        {
            if (!GetBoards().Any(b => b.Id == taskFormModel.BoardId))
            {
                ModelState.AddModelError(nameof(taskFormModel.BoardId), "Board does not exist.");
            }

            var currentUserId = GetUserId();
            var task = new Task
            {
                Title = taskFormModel.Title,
                Description = taskFormModel.Description,
                CretedOn = DateTime.Now,
                BoardId = taskFormModel.BoardId,
                OwnerId = currentUserId
            };
            data.Tasks.Add(task);
            data.SaveChanges();

            var boards = data.Boards;

            return RedirectToAction("All", "Boards");
        }

        public IActionResult Details(int id)
        {
            var task = data.Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskDetailsViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedOn = t.CretedOn.ToString("dd/MM/yyyy HH:mm"),
                    Board = t.Board.Name,
                    Owner = t.Owner.UserName
                })
                .FirstOrDefault();

            if (task == null)
            {
                return BadRequest();
            }

            return View(task);
        }

        public IActionResult Edit(int id)
        {
            var task = data.Tasks.Find(id);
            if (task == null)
            {
                return BadRequest();
            }

            var currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            var taskModel = new TaskFormModel
            {
                Title = task.Title,
                Description = task.Description,
                BoardId = task.BoardId,
                Boards = GetBoards()
            };

            return View(taskModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, TaskFormModel taskModel)
        {
            var task = data.Tasks.Find(id);
            if(task == null)
            {
                return BadRequest();
            }

            var currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            if (!GetBoards().Any(b => b.Id == taskModel.BoardId))
            {
                ModelState.AddModelError(nameof(taskModel.BoardId), "Board does not exist.");
            }

            task.Title = taskModel.Title;
            task.Description = taskModel.Description;
            task.BoardId = taskModel.BoardId;

            data.SaveChanges();
            return RedirectToAction("All", "Boards");
        }

        public IActionResult Delete(int id)
        {
            var task = data.Tasks.Find(id);
            if (task == null)
            {
                return BadRequest();
            }

            var currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            var taskViewModel = new TaskViewModel
            {
                Id = task.Id,
                Description = task.Description,
                Title = task.Title
            };

            return View(taskViewModel);
        }

        [HttpPost]
        public IActionResult Delete(TaskViewModel taskViewModel)
        {
            var task = data.Tasks.Find(taskViewModel.Id);
            if (task == null)
            {
                return BadRequest();
            }

            var currentUserId = GetUserId();
            if (currentUserId != task.OwnerId)
            {
                return Unauthorized();
            }

            data.Tasks.Remove(task);
            data.SaveChanges();

            return RedirectToAction("All", "Boards");
        }

        private IEnumerable<TaskBoardModel> GetBoards()
            => data
                .Boards
                .Select(b => new TaskBoardModel
                {
                    Id = b.Id,
                    Name = b.Name,
                });

        private string GetUserId()
            => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

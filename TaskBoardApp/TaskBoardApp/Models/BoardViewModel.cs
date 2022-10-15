using TaskBoardApp.Models.Task;

namespace TaskBoardApp.Models
{
    public class BoardViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TaskViewModel> Tasks { get; set; }
    }
}

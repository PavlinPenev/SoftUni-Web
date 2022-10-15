using System.ComponentModel.DataAnnotations;
using static TaskBoardApp.Data.DataConstants.Board;

namespace TaskBoardApp.Data.Entities
{
    public class Board
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    }
}

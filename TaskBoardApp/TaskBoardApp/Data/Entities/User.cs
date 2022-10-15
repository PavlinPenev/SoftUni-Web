using Microsoft.AspNetCore.Identity;
using static TaskBoardApp.Data.DataConstants.User;
using System.ComponentModel.DataAnnotations;

namespace TaskBoardApp.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(FIRST_NAME_MAX_LENGTH)]
        public string FirstName { get; init; }

        [Required]
        [MaxLength(LAST_NAME_MAX_LENGTH)]
        public string LastName { get; init; }
    }
}

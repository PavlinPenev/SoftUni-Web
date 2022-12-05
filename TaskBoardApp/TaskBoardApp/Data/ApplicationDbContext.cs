using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = TaskBoardApp.Data.Entities.Task;
using TaskBoardApp.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace TaskBoardApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private User GuestUser { get; set; }
        private Board OpenBoard { get; set; }
        private Board InProgressBoard { get; set; }
        private Board DoneBoard { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<Board> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Task>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            SeedUsers();

            builder.Entity<User>()
                .HasData(GuestUser);

            SeedBoards();

            builder.Entity<Board>()
                .HasData(OpenBoard, InProgressBoard, DoneBoard);

            builder.Entity<Task>()
                .HasData(
                new Task
                {
                    Id = 1,
                    Title = "Prepare for ASP.NET Fundamentals exam",
                    Description = "Learn using ASP.NET Core Identity",
                    CretedOn = DateTime.Now.AddMonths(-1),
                    OwnerId = GuestUser.Id,
                    BoardId = OpenBoard.Id
                },
                new Task
                {
                    Id = 2,
                    Title = "Improve EF Core skills",
                    Description = "Learn using EF Core and MS SQL Server Management Studio",
                    CretedOn = DateTime.Now.AddMonths(-5),
                    OwnerId = GuestUser.Id,
                    BoardId = DoneBoard.Id
                },
                new Task
                {
                    Id = 3,
                    Title = "Imporve ASP.NET Core skills",
                    Description = "Learn using ASP.NET Core Identity",
                    CretedOn = DateTime.Now.AddDays(-10),
                    OwnerId = GuestUser.Id,
                    BoardId = InProgressBoard.Id
                },
                new Task
                {
                    Id = 4,
                    Title = "Prepare for C# Fundamentals exam",
                    Description = "Prepare by solving old Mid and Final exams",
                    CretedOn = DateTime.Now.AddYears(-1),
                    OwnerId = GuestUser.Id,
                    BoardId = DoneBoard.Id
                });


            base.OnModelCreating(builder);
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<IdentityUser>();

            GuestUser = new User
            {
                UserName = "guest",
                NormalizedUserName = "GUEST",
                Email = "guest@mail.com",
                NormalizedEmail = "GUEST@MAIL.COM",
                FirstName = "Guest",
                LastName = "User"
            };

            GuestUser.PasswordHash = hasher.HashPassword(GuestUser, "guest");
        }

        private void SeedBoards()
        {
            OpenBoard = new Board
            {
                Id = 1,
                Name = "Open"
            };

            InProgressBoard = new Board
            {
                Id = 2,
                Name = "In Progress"
            };

            DoneBoard = new Board
            {
                Id = 3,
                Name = "Done"
            };
        }
    }
}
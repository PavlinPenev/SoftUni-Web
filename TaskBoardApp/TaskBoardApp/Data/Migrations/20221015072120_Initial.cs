using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardApp.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Board",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CretedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Task_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6d07d05e-eb21-4a29-b3fc-9136c4631082", 0, "c23960b5-bd83-4f72-8557-0f342dd9e8bf", "guest@mail.com", false, "Guest", "User", false, null, "GUEST@MAIL.COM", "GUEST", "AQAAAAEAACcQAAAAENEGtPpYldqL5oOD9Kc2s5FRbJFxgEo/74i9C0CdyhbEsbLNV9DqoiPj0mpgtuTZ+A==", null, false, "79e7f86e-17c6-495f-91d7-0304ed6d5074", false, "guest" });

            migrationBuilder.InsertData(
                table: "Board",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Open" },
                    { 2, "In Progress" },
                    { 3, "Done" }
                });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "BoardId", "CretedOn", "Description", "OwnerId", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2022, 9, 15, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6253), "Learn using ASP.NET Core Identity", "6d07d05e-eb21-4a29-b3fc-9136c4631082", "Prepare for ASP.NET Fundamentals exam" },
                    { 2, 3, new DateTime(2022, 5, 15, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6288), "Learn using EF Core and MS SQL Server Management Studio", "6d07d05e-eb21-4a29-b3fc-9136c4631082", "Improve EF Core skills" },
                    { 3, 2, new DateTime(2022, 10, 5, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6292), "Learn using ASP.NET Core Identity", "6d07d05e-eb21-4a29-b3fc-9136c4631082", "Imporve ASP.NET Core skills" },
                    { 4, 3, new DateTime(2021, 10, 15, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6295), "Prepare by solving old Mid and Final exams", "6d07d05e-eb21-4a29-b3fc-9136c4631082", "Prepare for C# Fundamentals exam" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Task_BoardId",
                table: "Task",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Task_OwnerId",
                table: "Task",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Board");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d07d05e-eb21-4a29-b3fc-9136c4631082");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");
        }
    }
}

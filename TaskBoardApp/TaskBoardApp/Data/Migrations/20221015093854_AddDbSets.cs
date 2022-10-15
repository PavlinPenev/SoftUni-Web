using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoardApp.Data.Migrations
{
    public partial class AddDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_AspNetUsers_OwnerId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Board_BoardId",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Board");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d07d05e-eb21-4a29-b3fc-9136c4631082");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameTable(
                name: "Board",
                newName: "Boards");

            migrationBuilder.RenameIndex(
                name: "IX_Task_OwnerId",
                table: "Tasks",
                newName: "IX_Tasks_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_BoardId",
                table: "Tasks",
                newName: "IX_Tasks_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Boards",
                table: "Boards",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e7fcfe37-6db0-40f6-a351-9048893c53b5", 0, "3c420c61-119f-4cd9-b4a0-92c9900bcc8e", "guest@mail.com", false, "Guest", "User", false, null, "GUEST@MAIL.COM", "GUEST", "AQAAAAEAACcQAAAAECn65arou1p5oJpG5U5yGk1LZJH8WWlA2QWFx/J+MqpHiMIvzhkCwCU+9JV5nQED2g==", null, false, "0a5129c6-e4b5-468f-8b81-e8de1a541b51", false, "guest" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2022, 9, 15, 12, 38, 53, 619, DateTimeKind.Local).AddTicks(682), "e7fcfe37-6db0-40f6-a351-9048893c53b5" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2022, 5, 15, 12, 38, 53, 619, DateTimeKind.Local).AddTicks(716), "e7fcfe37-6db0-40f6-a351-9048893c53b5" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2022, 10, 5, 12, 38, 53, 619, DateTimeKind.Local).AddTicks(720), "e7fcfe37-6db0-40f6-a351-9048893c53b5" });

            migrationBuilder.UpdateData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2021, 10, 15, 12, 38, 53, 619, DateTimeKind.Local).AddTicks(724), "e7fcfe37-6db0-40f6-a351-9048893c53b5" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_OwnerId",
                table: "Tasks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Boards_BoardId",
                table: "Tasks",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_OwnerId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Boards_BoardId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Boards",
                table: "Boards");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e7fcfe37-6db0-40f6-a351-9048893c53b5");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameTable(
                name: "Boards",
                newName: "Board");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_OwnerId",
                table: "Task",
                newName: "IX_Task_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_BoardId",
                table: "Task",
                newName: "IX_Task_BoardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Board",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6d07d05e-eb21-4a29-b3fc-9136c4631082", 0, "c23960b5-bd83-4f72-8557-0f342dd9e8bf", "guest@mail.com", false, "Guest", "User", false, null, "GUEST@MAIL.COM", "GUEST", "AQAAAAEAACcQAAAAENEGtPpYldqL5oOD9Kc2s5FRbJFxgEo/74i9C0CdyhbEsbLNV9DqoiPj0mpgtuTZ+A==", null, false, "79e7f86e-17c6-495f-91d7-0304ed6d5074", false, "guest" });

            migrationBuilder.UpdateData(
                table: "Task",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2022, 9, 15, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6253), "6d07d05e-eb21-4a29-b3fc-9136c4631082" });

            migrationBuilder.UpdateData(
                table: "Task",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2022, 5, 15, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6288), "6d07d05e-eb21-4a29-b3fc-9136c4631082" });

            migrationBuilder.UpdateData(
                table: "Task",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2022, 10, 5, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6292), "6d07d05e-eb21-4a29-b3fc-9136c4631082" });

            migrationBuilder.UpdateData(
                table: "Task",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CretedOn", "OwnerId" },
                values: new object[] { new DateTime(2021, 10, 15, 10, 21, 18, 142, DateTimeKind.Local).AddTicks(6295), "6d07d05e-eb21-4a29-b3fc-9136c4631082" });

            migrationBuilder.AddForeignKey(
                name: "FK_Task_AspNetUsers_OwnerId",
                table: "Task",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Board_BoardId",
                table: "Task",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

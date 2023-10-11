using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taskify.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskDueDetails_TaskItem_TaskItemId",
                table: "TaskDueDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDueDetails_TaskItem_TaskItemId",
                table: "TaskDueDetails",
                column: "TaskItemId",
                principalTable: "TaskItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskDueDetails_TaskItem_TaskItemId",
                table: "TaskDueDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskDueDetails_TaskItem_TaskItemId",
                table: "TaskDueDetails",
                column: "TaskItemId",
                principalTable: "TaskItem",
                principalColumn: "Id");
        }
    }
}
